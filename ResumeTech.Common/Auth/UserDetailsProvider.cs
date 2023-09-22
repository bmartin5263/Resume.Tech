using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Common.Auth; 

public class UserDetailsProvider : IUserDetailsProvider {
    private UserDetails? currentUser;

    public UserDetailsProvider() {
        Console.WriteLine("New UserDetailsProvider");
    }

    public UserDetails CurrentUser {
        get {
            return currentUser.OrElseThrow(() => new InvalidOperationException("UserDetails has not been set"));
        }
    }

    public void Set(UserDetails details) {
        if (currentUser != null) {
            throw new InvalidOperationException("Cannot set UserId twice");
        }
        currentUser = details;
    }
}