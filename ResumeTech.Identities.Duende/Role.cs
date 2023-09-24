using Microsoft.AspNetCore.Identity;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende; 

public class Role : IdentityRole<UserId>, IRole {
    public sealed override UserId Id { get; set; }
    public RoleName RoleName => Name!.ParseEnumOrThrow<RoleName>();

    public Role() {
    }

    public Role(string roleName) : base(roleName) {
    }

    public Role(UserId Id, RoleName RoleName) : base(RoleName.ToString()) {
        this.Id = Id;
        
    }
    
}