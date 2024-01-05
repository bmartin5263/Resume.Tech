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

public class Register : Command<RegisterRequest, UserDto> {
    private static readonly ILogger Log = Logging.CreateLogger<Register>();
    public override string Name => "Register";
    public override Roles UserRoles { get; } = Roles.Public();

    private IUserManager UserManager { get; }
    private UserOptions UserOptions { get; }

    public Register(IUserManager userManager, UserOptions userOptions) {
        UserManager = userManager;
        UserOptions = userOptions;
    }

    public override async Task Validate(ValidationContext<RegisterRequest> ctx) {
        var request = ctx.GetRequest();
        
        var emailExists = await UserManager.UserExistsByEmail(request.Email);
        if (emailExists) {
            ctx.AddError("email", "An account with this email already exists");
        }

        var userExists = await UserManager.UserExistsByUsername(request.Username);
        if (userExists) {
            ctx.AddError("username", "An account with this username already exists");
        }
    }

    public override async Task<UserDto> Run(RegisterRequest args) {
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