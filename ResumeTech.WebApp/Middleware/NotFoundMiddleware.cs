using System.Net;
using ResumeTech.Common.Error;

namespace ResumeTech.Application.Middleware; 

public class NotFoundMiddleware {
    private readonly RequestDelegate next;

    public NotFoundMiddleware(RequestDelegate next) {
        this.next = next;
    }

    
    public async Task Invoke(HttpContext context) {
        await next(context);
        if (context.Response.StatusCode == 404) {
            throw InvalidUriException.UriNotFound(context.Request.Path);
        }
    }
}