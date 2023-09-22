using ResumeTech.Common.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Middleware; 

public class SetUserMiddleware {
    private RequestDelegate Next { get; }
    
    public SetUserMiddleware(RequestDelegate next) {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserDetailsProvider userDetailsProvider) {
        Console.WriteLine("Setting User...");
        if (context.Request.Headers.TryGetValue("UserId", out var userId)) {
            try {
                userDetailsProvider.Set(new UserDetails(Id: UserId.Parse(userId[0]!), Roles: null));
            }
            catch (Exception e) {
                Console.WriteLine(e);
                userDetailsProvider.Set(new UserDetails(Id: null, Roles: null));
            }
        }
        else {
            userDetailsProvider.Set(new UserDetails(Id: null, Roles: null));
        }
        await Next(context);
    }
}