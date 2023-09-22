using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Users; 

public record CreateRoleRequest(UserId Id, RoleName Name);