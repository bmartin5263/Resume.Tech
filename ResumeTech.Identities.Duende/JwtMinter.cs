using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Options;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Duende; 

public class JwtMinter: IJwtMinter {
    private JwtOptions Options { get; }

    public JwtMinter(JwtOptions options) {
        Options = options;
    }

    public Jwt MintToken(IUser user, IList<IRole> roles) {
        var authClaims = new List<Claim> {
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        authClaims.AddRange(roles.Select(userRole => new Claim("role", userRole.RoleName.ToString())));
        
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Key));
        var token = new JwtSecurityToken(
            issuer: Options.Issuer,
            audience: Options.Audience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
        return new Jwt(tokenAsString);
    }
    
}