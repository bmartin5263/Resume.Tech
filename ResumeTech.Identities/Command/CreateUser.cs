using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Command; 

public class CreateUser : Command<CreateUserRequest, UserDto> {
    public override string Name => "CreateUser";
    public override RoleName[] Roles { get; } = { RoleName.Admin };

    private IUserManager UserManager { get; }

    public CreateUser(IUserManager userManager) {
        UserManager = userManager;
    }

    public override async Task<UserDto> Run(CreateUserRequest args) {
        var user = await UserManager.CreateUserAsync(args);
        return user.ToDto();
    }
}