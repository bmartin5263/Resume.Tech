using Microsoft.AspNetCore.Identity;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende;

public class User : IdentityUser<UserId>, IUser {
    
    public sealed override UserId Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? RefreshToken { get; set; }
    public EmailAddress EmailAddress => new(Email!);

    protected User() {
        Id = UserId.Generate();
    }

    public User(string userName) : base(userName) {
        Id = UserId.Generate();
    }
}