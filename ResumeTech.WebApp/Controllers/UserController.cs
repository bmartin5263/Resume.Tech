using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Command;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Controllers; 

[ApiController]
public class UserController : ControllerBase {
    private Exec Exec { get; }
    private IUnitOfWork UnitOfWork { get; }
    private IUserProvider UserProvider { get; }

    public UserController(Exec exec, IUnitOfWork unitOfWork, IUserProvider userProvider) {
        Exec = exec;
        UnitOfWork = unitOfWork;
        UserProvider = userProvider;
    }

    /// <summary>
    /// Login a User, returning a JWT
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login(
        [FromHeader(Name = "Authorization")] string authorization
    ) {
        ParseBasicAuth(authorization, out string usernameOrEmail, out string password);
        var command = UnitOfWork.GetService<Login>();
        var result = await Exec.Command(command, new LoginParameters(
            UsernameOrEmail: usernameOrEmail,
            Password: password
        ));
        
        Response.Cookies.Append("X-Access-Token", result!.Token.Value, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
        // Response.Cookies.Append("X-Username", user.UserName, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
        // Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

        return Ok(new LoginResponse(
            Token: result.Token,
            CurrentTime: result.CurrentTime,
            ExpiresAt: result.ExpiresAt,
            UserId: result.UserId
        ));
    }
    
    /// <summary>
    /// Login a User, returning a JWT
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<UserDto> Register([FromBody] RegisterParameters args) {
        var command = UnitOfWork.GetService<Register>();
        return await Exec.Command(command, args);
    }
    
    private static void ParseBasicAuth(string header, out string username, out string password) {
        var token = header["Basic ".Length..].Trim();
        var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialsAsEncodedString.Split(':');
        username = credentials[0];
        password = credentials[1];
    }

    
}