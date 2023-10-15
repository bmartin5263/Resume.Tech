using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.TestUtil; 

public class UserDetailsProvider : IUserDetailsProvider {
    public UserDetails CurrentUser { get; }
    public UserId CurrentUserId => CurrentUser.Id!.Value.OrElseThrow();

    public UserDetailsProvider(UserDetails currentUser) {
        CurrentUser = currentUser;
    }

    public void Set(UserDetails userDetails) {
        throw new InvalidOperationException("Cannot set CurrentUser twice");
    }
}