using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.TestUtil; 

public class UserProvider : IUserProvider {
    public UserDetails CurrentUser { get; }
    public UserId CurrentUserId => CurrentUser.Id!.Value.OrElseThrow();

    public UserProvider(UserDetails currentUser) {
        CurrentUser = currentUser;
    }

    public void Set(UserDetails userDetails) {
        throw new InvalidOperationException("Cannot set CurrentUser twice");
    }
}