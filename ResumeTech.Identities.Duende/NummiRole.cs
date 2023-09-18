using Microsoft.AspNetCore.Identity;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende; 

public class Role : IdentityRole<UserId>, IRole {
    public Role() { }
    public Role(string roleName) : base(roleName) { }
}