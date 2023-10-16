using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Console;
using ResumeTech.Application.Cqs;
using ResumeTech.Application.Middleware;
using ResumeTech.Application.Serialization;
using ResumeTech.Application.Util;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application;

internal class Program {
    public const string RootAssembly = "ResumeTech";

    protected Program() {
        
    }
    
    public static async Task Main(string[] args) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Identities"));
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Experiences"));

        var builder = WebApplication.CreateBuilder(args);
        var userOptions = builder.BindOptions<UserOptions>("User");
        var securityOptions = builder.BindOptions<SecurityOptions>("Security");
        builder.BindOptions<JwtOptions>("Security:JWT");
        var databaseOptions = builder.BindOptions<DatabaseOptions>("Database");
        builder.BindOptions<WebOptions>("Web");
        var mappedTypes = TypeMapping.GenerateTypeMappings();

        builder.ConfigureJson(mappedTypes);
        builder.ConfigureDatabase(databaseOptions);
        builder.ConfigureSwagger(mappedTypes);
        builder.ConfigureAuthentication(userOptions, securityOptions);
        builder.ConfigureCors();

        var commandsAndQueries = builder.AutoAddServices();
        builder.ManuallyAddServices(commandsAndQueries);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging(logging =>
            logging.AddSimpleConsole(options => {
                options.SingleLine = true;
                options.ColorBehavior = LoggerColorBehavior.Disabled;
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                options.UseUtcTimestamp = true;
            })
        );
        
        builder.Services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = actionContext =>
            {
                var parameters = actionContext.ModelState;
                var errorBuilder = AppError.Builder(HttpStatusCode.BadRequest);

                foreach (var (key, value) in parameters) {
                    foreach (var errorMessage in value.Errors.Select(e => e.ErrorMessage)) {
                        if (errorMessage.EndsWith("is required.")) {
                            errorBuilder.SubError(new AppSubError(
                                Path: char.ToLowerInvariant(key[0]) + key[1..],
                                Message: "Value is required"
                            ));
                        }
                        else {
                            errorBuilder.SubError(new AppSubError(
                                Path: char.ToLowerInvariant(key[0]) + key[1..],
                                Message: errorMessage
                            ));
                        }
                    }
                }

                throw new AppException(errorBuilder.Build());
            };
        });
        
        var app = builder.Build();
        Logging.LoggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandler(new ExceptionHandlerOptions {
            ExceptionHandler = new JsonExceptionMiddleware(app.Environment.IsEnvironment("Local")).Invoke
        });
        app.UseMiddleware<NotFoundMiddleware>();
        app.UseAuthorization();
        app.MapControllers();
        
        if (builder.Environment.EnvironmentName != "Test") {
            app.MigrateDb();
        }
        
        await Initialize(app);
        await app.RunAsync();
    }

    private static async Task Initialize(WebApplication app) {
        Console.WriteLine("Initializing");
        var exec = app.Services.GetRequiredService<Exec>();
        var userDetailsProvider = app.Services.GetRequiredService<IUserProvider>();
        userDetailsProvider.Login(UserDetails.SystemUser);
        
        var unitOfWork = app.Services.GetRequiredService<IUnitOfWork>();
        await exec.Command(unitOfWork.GetService<InitializeApp>());
    }
}