using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Events;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende; 

public class DuendeUserManager : IUserManager {
    private static readonly ILogger Log = Logging.CreateLogger<DuendeUserManager>();

    private UserManager<User> UserManager { get; }
    private RoleManager<Role> RoleManager { get; }
    private WebOptions WebOptions { get; }
    private IUnitOfWork UnitOfWork { get; }

    public DuendeUserManager(
        UserManager<User> userManager, 
        RoleManager<Role> roleManager, 
        WebOptions webOptions,
        IUnitOfWork unitOfWork
    ) {
        UserManager = userManager;
        RoleManager = roleManager;
        WebOptions = webOptions;
        UnitOfWork = unitOfWork;
    }

    public Task<bool> RoleExistsAsync(RoleName roleName) {
        return RoleManager.RoleExistsAsync(roleName.ToString());
    }

    public async Task<bool> UserExistsByUsername(string username) {
        var user = await UserManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<bool> UserExistsByEmail(EmailAddress email) {
        var user = await UserManager.FindByEmailAsync(email.Value);
        return user != null;
    }

    public async Task<IUser?> FindUserByUsernameAsync(string username) {
        var user = await UserManager.FindByNameAsync(username);
        return user;
    }

    public async Task<IUser?> FindUserByIdWithRefreshTokenAsync(UserId userId, Guid refreshToken) {
        var user = await UserManager.FindByIdAsync(userId.Value.ToString());
        return user?.RefreshToken == refreshToken ? user : null;
    }

    public async Task<IUser?> FindUserByIdAsync(UserId userId) {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        return user;
    }

    public async Task<IUser> CreateUserAsync(CreateUserRequest request) {
        return await NewUser(request);
    }

    private async Task<User> NewUser(CreateUserRequest request) {
        var user = new User(request.Username) {
            Id = request.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            Email = request.Email.Value,
            EmailConfirmed = request.EmailConfirmed,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        Log.LogInformation($"Created User(Name={user.UserName}, EmailConfirmed={user.EmailConfirmed})");
        var result = await UserManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) {
            throw Errors.Builder(HttpStatusCode.BadRequest)
                .SubErrors(result.Errors
                    .Where(e => e.Code.StartsWith("Password"))
                    .Select(e => new AppSubError(
                        Path: e.Code.StartsWith("Password") ? "password" : "username",
                        Message: $"{e.Description}"
                    ))
                )
                .ToException();
        }
        
        result = await UserManager.AddToRoleAsync(user, RoleName.User.ToString());
        if (!result.Succeeded) {
            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new AppException($"Failed to assign 'User' role to new user. Error(s): {error}");
        }
        UnitOfWork.RaiseEvent(new UserCreatedEvent(request.Email, request.EmailConfirmed));

        return user;
    }

    public async Task CreateRoleAsync(CreateRoleRequest request) {
        await RoleManager.CreateAsync(new Role(Id: request.Id, RoleName: request.Name));
    }

    public async Task SendConfirmEmailMessage(EmailAddress emailAddress) {
        var user = (await UserManager.FindByEmailAsync(emailAddress.Value))
            .OrElseThrow(() => Errors.EntityNotFound("User", emailAddress.Value).ToException());

        if (user.EmailConfirmed) {
            throw Errors.Builder(HttpStatusCode.BadRequest)
                .UserMessage("User's email is already confirmed")
                .ToException();
        }
        
        // await SendEmail(EmailTemplateName.ConfirmYourEmail, user);
    }
    
    // public async Task SendPasswordResetEmail(string usernameOrEmail) {
    //     User user = await FetchUser(usernameOrEmail);
    //     await SendEmail(EmailTemplateName.ResetYourPassword, user);
    // }

    // private async Task SendEmail(EmailTemplateName template, User user) {
    //     var token = await UserManager.GeneratePasswordResetTokenAsync(user);
    //     var tokenEncoded = token.ToBase64();
    //     
    //     var emailTemplate = EmailOptions.GetTemplate(template);
    //     var message = new Email(
    //         sender: SenderIdentity,
    //         recipient: new EmailIdentity(user.Email!, user.Name),
    //         subject: emailTemplate.Subject,
    //         htmlContent: Resources.ReadFile(emailTemplate.FileBody()).TokenReplace(new Dictionary<string, object?> {
    //             {"URL", WebOptions.FrontendUrl},
    //             {"EMAIL", user.Email},
    //             {"TOKEN", tokenEncoded}
    //         })
    //     );
    //     await EmailSender.Send(message);
    // }

    private async Task<User> FetchUser(string usernameOrEmail) {
        User? user = await UserManager.FindByNameAsync(usernameOrEmail) 
                          ?? await UserManager.FindByEmailAsync(usernameOrEmail);

        if (user != null) {
            return user;
        }
        
        var label = usernameOrEmail.Contains('@') ? "email" : "username";
        throw Errors.Builder(HttpStatusCode.BadRequest)
            .SubError("username", message: $"No User associated with {label} {usernameOrEmail}")
            .ToException();
    }

    public async Task ResetPassword(EmailAddress emailAddress, string token, string newPassword) {
        User user = await FetchUser(emailAddress.Value);

        var result = await UserManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded) {
            var invalidTokenError = result.Errors.FirstOrDefault(e => e.Code.StartsWith("InvalidToken"));
            if (invalidTokenError != null) {
                throw Errors.Builder(HttpStatusCode.BadRequest)
                    .UserMessage("Unable to reset password at this time")
                    .ToException();
            }
            throw Errors.Builder(HttpStatusCode.BadRequest)
                .SubErrors(result.Errors
                    .Where(e => e.Code.StartsWith("Password"))
                    .Select(e => new AppSubError(
                        Path: e.Code.StartsWith("Password") ? "password" : "username",
                        Message: $"{e.Description}"
                    ))
                )
                .ToException();
        }
    }

    public async Task ConfirmEmail(EmailAddress emailAddress, string token) {
        var user = (await UserManager.FindByEmailAsync(emailAddress.Value))
            .OrElseThrow(() => Errors.EntityMissing<IUser>(emailAddress.Value).ToException());
        
        var result = await UserManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) {
            Log.LogError("Failed to Confirm Email. Errors: {}", result.Errors.Select(e => e.Description).Aggregate((a, b) => a + ", " + b));
        }
    }

    public async Task AssignRoleAsync(IUser user, RoleName roleName) {
        await UserManager.AddToRoleAsync((User) user, roleName.ToString());
    }

    public async Task<IUser> LoginAsync(string usernameOrEmail, string password) {
        User? user = await UserManager.FindByNameAsync(usernameOrEmail) 
                          ?? await UserManager.FindByEmailAsync(usernameOrEmail);

        if (user == null) {
            Log.LogInformation("Missing User");
            var label = usernameOrEmail.Contains('@') ? "email" : "username";
            throw Errors.Unauthorized($"No account associated with {label} {usernameOrEmail}").ToException();
        }

        if (!await UserManager.IsEmailConfirmedAsync(user)) {
            Log.LogInformation("User Email Not Confirmed");
            // var urlWithoutHttp = WebOptions.FrontendUrl.StripHttpProtocol();
            throw Errors.Unauthorized().ToException();
            // throw Errors.Unauthorized($"Email has not been confirmed. To resend email go to {urlWithoutHttp}/resend-email").ToException();
        }

        if (await UserManager.IsLockedOutAsync(user)) {
            Log.LogInformation("User Locked Out");
            throw Errors.Unauthorized($"User is locked out until {user.LockoutEnd} ({user.LockoutEnd - DateTimeOffset.UtcNow}) due to password attempt failures").ToException();
        }

        if (!await UserManager.CheckPasswordAsync(user, password)) {
            Log.LogInformation("Password Check Failed");
            await UserManager.AccessFailedAsync(user);
            throw Errors.Unauthorized($"Incorrect password").ToException();
        }
        
        if (await UserManager.GetAccessFailedCountAsync(user) > 0) {
            await UserManager.ResetAccessFailedCountAsync(user);
        }
        
        return user;
    }

    public async Task<IList<IRole>> GetRolesAsync(IUser user) {
        IList<IRole> allRoles = RoleManager.Roles.Select(r => (IRole) r).ToList();
        ISet<string> assignedRoles = (await UserManager.GetRolesAsync((User) user)).ToHashSet();
        return allRoles.Where(r => assignedRoles.Contains(r.RoleName.ToString())).ToList();
    }
}