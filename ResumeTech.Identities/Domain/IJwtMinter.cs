using System.Security.Claims;
using ResumeTech.Common.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Domain; 

public interface IJwtMinter {
    public Jwt MintToken(IUser user, IList<IRole> roles);
}