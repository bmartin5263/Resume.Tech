using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Identities.Users; 

public class UserDetails {
    public static UserDetails NotLoggedIn { get; } = new(null, null);
    
    public UserId? Id { get; }
    public IReadOnlySet<RoleName> Roles { get; }

    public UserDetails(UserId? Id, ISet<RoleName>? Roles) {
        this.Id = Id;
        this.Roles = Roles.ToReadOnly();
    }
}