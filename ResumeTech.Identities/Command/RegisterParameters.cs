using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Command;

public sealed record RegisterParameters(string Username, EmailAddress Email, string Password);
