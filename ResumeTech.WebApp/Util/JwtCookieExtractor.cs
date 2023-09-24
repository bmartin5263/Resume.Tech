using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ResumeTech.Application.Util; 

public class JwtCookieExtractor : JwtBearerEvents {
    public override Task MessageReceived(MessageReceivedContext context) {
        if (context.Request.Cookies.ContainsKey("X-Access-Token")) {
            context.Token = context.Request.Cookies["X-Access-Token"];
        }
        return Task.CompletedTask;
    }
}