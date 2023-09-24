using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Users; 

public sealed record CreateUserRequest(
    UserId Id,
    string Username,
    string Password,
    EmailAddress Email,
    bool EmailConfirmed
);