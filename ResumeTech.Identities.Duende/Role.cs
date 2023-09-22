using Microsoft.AspNetCore.Identity;
using ResumeTech.Common.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende; 

public class Role : IdentityRole<UserId>, IRole {
    public Role() {
        
    }

    public Role(string roleName) : base(roleName) {
        
    }

    public Role(UserId Id, RoleName RoleName) : base(RoleName.ToString()) {
        
    }
}