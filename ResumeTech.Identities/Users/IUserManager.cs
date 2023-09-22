using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Users; 

public interface IUserManager {
    public Task<bool> RoleExistsAsync(RoleName roleName);
    public Task<bool> UserExistsByUsername(string username);
    public Task<bool> UserExistsByEmail(EmailAddress email);
    
    public Task<IUser?> FindUserByUsernameAsync(string username);
    public Task<IUser?> FindUserByIdAsync(UserId userId);
    public Task<IUser?> FindUserByIdWithRefreshTokenAsync(UserId userId, Guid refreshToken);
    public Task<ISet<RoleName>> GetRolesAsync(IUser user);

    public Task<IUser> LoginAsync(string usernameOrEmail, string password);
    
    public Task<IUser> CreateUserAsync(CreateUserRequest request);
    public Task CreateRoleAsync(CreateRoleRequest request);
    public Task AssignRoleAsync(IUser user, RoleName roleName);

    // public Task SendConfirmEmailMessage(EmailAddress emailAddress);
    // public Task ConfirmEmail(EmailAddress emailAddress, string token);
    //
    // public Task SendPasswordResetEmail(string usernameOrEmail);
    // public Task ResetPassword(EmailAddress emailAddress, string token, string newPassword);
}