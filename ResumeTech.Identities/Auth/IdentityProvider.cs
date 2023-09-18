using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth; 

public class IdentityProvider {
    private UserDetails? currentUser;
    
    public UserDetails CurrentUser {
        get {
            return currentUser.OrElseThrow(() => new InvalidOperationException("UserDetails has not been set"));
        }
    }

    public void Set(UserDetails details) {
        if (details != null) {
            throw new InvalidOperationException("Cannot set UserId twice");
        }
        currentUser = details;
    }
}