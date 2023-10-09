using ResumeTech.Common.Auth;

namespace ResumeTech.TestUtil; 

public class UserDetailsProvider : IUserDetailsProvider {
    public UserDetails CurrentUser { get; }

    public UserDetailsProvider(UserDetails currentUser) {
        CurrentUser = currentUser;
    }

    public void Set(UserDetails userDetails) {
        throw new InvalidOperationException("Cannot set CurrentUser twice");
    }
}