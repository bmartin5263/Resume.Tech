using NLog;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Application.Middleware; 

public class SetUserMiddleware {
    private RequestDelegate Next { get; }
    
    public SetUserMiddleware(RequestDelegate next) {
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
        if (context.Request.Headers.TryGetValue("UserId", out var userId)) {
            try {
                context.RequestServices.GetRequiredService<UserIdProvider>().Set(UserId.Parse(userId[0]!));
            }
            catch (Exception) {
                // ignore
            }
        }
        await Next(context);
    }
}