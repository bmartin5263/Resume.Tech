namespace ResumeTech.Identities.Command;

public sealed record LoginRequest(string UsernameOrEmail, string Password);