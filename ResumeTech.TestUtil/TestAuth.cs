using ResumeTech.Common.Auth;

namespace ResumeTech.TestUtil; 

public static class TestAuth {

    public static IUserDetailsProvider RandomUserProvider() {
        return new UserDetailsProvider(new UserDetails(
            Id: UserId.Generate(),
            Username: "user",
            Roles: new HashSet<RoleName> { RoleName.User }
        ));
    }
    
}