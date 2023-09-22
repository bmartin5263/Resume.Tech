using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ResumeTech.Application.Middleware;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Duende;
using ResumeTech.Persistence.EntityFramework;
using UserOptions = ResumeTech.Common.Options.UserOptions;

namespace ResumeTech.Application.Util; 

public static class AuthenticationUtils {
    private const string CorsPolicyName = "CorsPolicy";

    public static void ConfigureAuthentication(
        this WebApplicationBuilder builder, 
        UserOptions userOptions,
        SecurityOptions securityOptions
    ) {
        builder.Services.AddDefaultIdentity<User>(options => {
                options.SignIn.RequireConfirmedAccount = userOptions.RequireConfirmedEmail;
                options.SignIn.RequireConfirmedEmail = userOptions.RequireConfirmedEmail;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddDefaultTokenProviders()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<EFCoreContext>();

        builder.Services.AddIdentityServer()
            .AddApiAuthorization<User, EFCoreContext>();

        builder.Services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
            })
            .AddJwtBearer(o => { 
                o.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = securityOptions.JWT.Issuer.OrElseThrow("Missing JWT Issuer"),
                    ValidAudience = securityOptions.JWT.Audience.OrElseThrow("Missing JWT Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityOptions.JWT.Key.OrElseThrow("Missing JWT Key"))),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                o.Events = new JwtCookieExtractor();
            });
    }

    public static void ConfigureCors(this WebApplicationBuilder builder) {
        builder.Services.AddCors(policyBuilder =>
            policyBuilder.AddPolicy(CorsPolicyName, policy =>
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000")
                )
        );
    }
}