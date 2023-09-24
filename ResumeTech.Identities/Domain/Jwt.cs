using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Domain; 

public record Jwt(string Value) : IWrapper<string>;