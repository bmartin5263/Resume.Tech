using ResumeTech.Common.Auth;
using ResumeTech.Common.Cqs;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Command; 

public class CreateUser : CqsCommand<CreateUserRequest, UserDto> {
    public override string Name => "CreateUser";
    public override bool AllowAnonymous => true;
    public override HashSet<RoleName> RequiresAnyRole => new() { RoleName.Admin };

    private IUserManager UserManager { get; }

    public CreateUser(IUserManager userManager) {
        UserManager = userManager;
    }

    public override async Task<UserDto> Execute(CreateUserRequest args) {
        var user = await UserManager.CreateUserAsync(args);
        return user.ToDto();
    }
}