using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Cqs; 

public class InitializeApp : PureCommand {
    public override string Name => "Initialize";
    public override RoleName[] Roles { get; } = Array.Empty<RoleName>();
    
    private IUserManager UserManager { get; }

    public InitializeApp(IUserManager userManager) {
        UserManager = userManager;
    }

    public override async Task RunWithoutResult() {
        var allRoles = Enum.GetValuesAsUnderlyingType<RoleName>();
        foreach (RoleName role in allRoles) {
            if (!await UserManager.RoleExistsAsync(role)) {
                await UserManager.CreateRoleAsync(new CreateRoleRequest(
                    Id: UserId.Generate(),
                    Name: role
                ));
            }
        }
        
        IUser admin = await UserManager.FindUserByUsernameAsync("admin") 
                       ?? await UserManager.CreateUserAsync(new CreateUserRequest(
                           Id: UserId.Generate(),
                           Username: "admin",
                           Email: new EmailAddress("admin@example.com"),
                           Password: "Password",
                           EmailConfirmed: true
                           )
                       );

        IList<IRole> roles = await UserManager.GetRolesAsync(admin);
        if (!roles.Select(r => r.RoleName).Contains(RoleName.Admin)) {
            await UserManager.AssignRoleAsync(admin, RoleName.Admin);
        }
        if (!roles.Select(r => r.RoleName).Contains(RoleName.User)) {
            await UserManager.AssignRoleAsync(admin, RoleName.User);
        }
    }
    
}