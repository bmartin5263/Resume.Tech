using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Users; 

public record CreateUserRequest(
    UserId Id,
    DateTimeOffset CreatedAt,
    string Username,
    string Password,
    EmailAddress Email,
    string SecurityStamp,
    bool EmailConfirmed
);