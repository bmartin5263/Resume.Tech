using System.Net;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Command;

public class Register : Command<RegisterParameters, UserDto> {
    private static readonly ILogger Log = Logging.CreateLogger<Register>();
    public override string Name => "Register";
    public override Roles UserRoles { get; } = Roles.Public();

    private IUserManager UserManager { get; }
    private UserOptions UserOptions { get; }

    public Register(IUserManager userManager, UserOptions userOptions) {
        UserManager = userManager;
        UserOptions = userOptions;
    }

    public override async Task<UserDto> Run(RegisterParameters args) {
        var emailExists = await UserManager.UserExistsByEmail(args.Email);
        if (emailExists) {
            throw AppError.Builder(HttpStatusCode.BadRequest)
                .SubError(new AppSubError(Path: "email", Message: "An account with this email already exists"))
                .ToException();
        }

        var userExists = await UserManager.UserExistsByUsername(args.Username);
        if (userExists) {
            throw AppError.Builder(HttpStatusCode.BadRequest)
                .SubError(new AppSubError(Path: "username", Message: "Username is already taken"))
                .ToException();
        }

        var createRequest = new CreateUserRequest(
            Id: UserId.Generate(),
            Email: args.Email,
            Username: args.Username,
            Password: args.Password,
            EmailConfirmed: UserOptions.RequireConfirmedEmail
        );
        var user = await UserManager.CreateUserAsync(createRequest);
        await UserManager.AssignRoleAsync(user, RoleName.User);

        return user.ToDto();
    }
}