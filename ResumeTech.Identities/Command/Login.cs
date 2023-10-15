using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Command;

public class Login : Command<LoginParameters, LoginResponse> {
    private static readonly ILogger Log = Logging.CreateLogger<Login>();
    public override string Name => "Login"; 
    public override Roles UserRoles { get; } = Roles.Public();

    private IUserManager UserManager { get; }
    private IJwtMinter JwtMinter { get; }

    public Login(IUserManager userManager, IJwtMinter jwtMinter) {
        UserManager = userManager;
        JwtMinter = jwtMinter;
    }

    public override async Task<LoginResponse> Run(LoginParameters args) {
        Log.LogInformation("Logging in {UsernameOrEmail}", args.UsernameOrEmail);
        
        var user = await UserManager.LoginAsync(args.UsernameOrEmail, args.Password);
        var userRoles = await UserManager.GetRolesAsync(user);

        var token = JwtMinter.MintToken(user, userRoles);
        return new LoginResponse(
            CurrentTime: DateTimeOffset.Now,
            ExpiresAt: DateTimeOffset.Now + TimeSpan.FromDays(3),
            Token: token,
            UserId: user.Id
        );
    }
}