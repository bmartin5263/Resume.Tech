using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Util; 

public class UserDetailsProvider : IUserDetailsProvider {
    private static readonly ILogger Log = Logging.CreateLogger<UserDetailsProvider>();
    public UserDetails CurrentUser { get; }

    public UserDetailsProvider(IHttpContextAccessor httpContextAccessor) {
        HttpContext? context = httpContextAccessor.HttpContext;
        if (context == null) {
            CurrentUser = UserDetails.NotLoggedIn;
            return;
        }
        
        string? username = null;
        UserId? userId = null;
        ISet<RoleName> roles = new HashSet<RoleName>();

        foreach (var claim in context.User.Claims) {
            if (claim.TypeContains("sub", "nameidentifier")) {
                userId = UserId.Parse(claim.Value);
            }
            else if (claim.TypeContains("role")) {
                roles.Add(claim.Value.ParseEnumOrThrow<RoleName>());
            }
            else if (claim.TypeContains("name")) {
                username = claim.Value;
            }
        }
        
        CurrentUser = new UserDetails(
            Id: userId,
            Username: username,
            Roles: roles
        );
    }

    public void Set(UserDetails details) {
        throw new InvalidOperationException("Cannot set UserId twice");
    }
}