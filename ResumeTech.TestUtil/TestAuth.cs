using ResumeTech.Common.Auth;

namespace ResumeTech.TestUtil; 

public static class TestAuth {

    public static IUserProvider RandomUserProvider() {
        return new UserProvider(new UserDetails(
            Id: UserId.Generate(),
            Username: "user",
            Roles: new HashSet<RoleName> { RoleName.User }
        ));
    }
    
}