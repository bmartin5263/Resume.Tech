using NLog;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Middleware; 

public class SetUserMiddleware {
    private RequestDelegate Next { get; }
    
    public SetUserMiddleware(RequestDelegate next) {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (context.Request.Headers.TryGetValue("UserId", out var userId)) {
            try {
                context.RequestServices.GetRequiredService<IdentityProvider>()
                    .Set(new UserDetails(Id: UserId.Parse(userId[0]!), Roles: null));
            }
            catch (Exception) {
                // ignore
            }
        }
        await Next(context);
    }
}