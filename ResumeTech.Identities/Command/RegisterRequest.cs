using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Command;

public sealed record RegisterRequest(string Username, EmailAddress Email, string Password);
