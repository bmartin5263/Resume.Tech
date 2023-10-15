using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Command; 

public class CreateUser : Command<CreateUserRequest, UserDto> {
    public override string Name => "CreateUser";
    public override Roles UserRoles { get; } = Roles.AdminOnly();

    private IUserManager UserManager { get; }

    public CreateUser(IUserManager userManager) {
        UserManager = userManager;
    }

    public override async Task<UserDto> Run(CreateUserRequest args) {
        var user = await UserManager.CreateUserAsync(args);
        return user.ToDto();
    }
}