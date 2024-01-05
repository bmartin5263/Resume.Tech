using ResumeTech.Common.Auth;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Identities.Command;

public sealed record LoginResponse(
    Jwt Token, 
    DateTimeOffset CurrentTime, 
    DateTimeOffset ExpiresAt,
    UserId UserId
);