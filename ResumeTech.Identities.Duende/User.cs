using Microsoft.AspNetCore.Identity;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende;

public class User : IdentityUser<UserId>, IUser {
    
    public sealed override UserId Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    protected User() {
        Id = UserId.Generate();
    }

    public User(string userName) : base(userName) {
        Id = UserId.Generate();
    }
}