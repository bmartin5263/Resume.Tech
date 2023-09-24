using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Users; 

public interface IRole {
    public UserId Id { get; }
    public RoleName RoleName { get; }
}