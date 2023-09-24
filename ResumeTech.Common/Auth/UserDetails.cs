using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Auth; 

public class UserDetails {
    public static UserDetails NotLoggedIn { get; } = new(null, null, null);
    
    public UserId? Id { get; }
    public string? Username { get; }
    public IReadOnlySet<RoleName> Roles { get; }

    public UserDetails(UserId? Id, string? Username, ISet<RoleName>? Roles) {
        this.Id = Id;
        this.Username = Username;
        this.Roles = Roles.ToReadOnly();
    }

    public bool IsAdmin() {
        return Roles.Contains(RoleName.Admin);
    }
}