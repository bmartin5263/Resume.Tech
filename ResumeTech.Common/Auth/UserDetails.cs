using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Auth; 

public class UserDetails {
    private static readonly Guid SystemUserId = Guid.NewGuid();
    
    public static UserDetails NotLoggedIn { get; } = new(null, null, null);
    public static UserDetails SystemUser { get; } = new(new UserId(SystemUserId), "System", new[] { RoleName.Admin });
    
    public UserId? Id { get; }
    public string? Username { get; }
    public IReadOnlySet<RoleName> Roles { get; }

    public UserDetails(UserId? Id, string? Username, IEnumerable<RoleName>? Roles) {
        this.Id = Id;
        this.Username = Username;
        this.Roles = new ReadOnlySet<RoleName>(Roles ?? Enumerable.Empty<RoleName>());
    }

    public bool IsAdmin() {
        return Roles.Contains(RoleName.Admin);
    }
}