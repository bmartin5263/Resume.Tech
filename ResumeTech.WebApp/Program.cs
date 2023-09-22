using System.Reflection;
using Microsoft.Extensions.Logging.Console;
using ResumeTech.Application.Middleware;
using ResumeTech.Application.Serialization;
using ResumeTech.Application.Util;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Cqs;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Command;
using ResumeTech.Identities.Users;

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
        var databaseOptions = builder.BindOptions<DatabaseOptions>("Database");
        var webOptions = builder.BindOptions<WebOptions>("Web");
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
            logging.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.ColorBehavior = LoggerColorBehavior.Disabled;
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
            })
        );

        var app = builder.Build();
        Logging.LoggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<SetUserMiddleware>();
        app.UseExceptionHandler(new ExceptionHandlerOptions {
            ExceptionHandler = new JsonExceptionMiddleware(app.Environment.IsEnvironment("Local")).Invoke
        });
        app.UseAuthorization();
        app.MapControllers();
        
        if (builder.Environment.EnvironmentName != "Test") {
            app.MigrateDb();
        }
        
        Console.WriteLine("Initializing");

        // try {
        //     var exec = app.Services.GetRequiredService<Exec>();
        //     var createUser = app.Services.GetRequiredService<CreateUser>();
        //     var userDetails = app.Services.GetRequiredService<IUserDetailsProvider>();
        //     userDetails.Set(UserDetails.NotLoggedIn);
        //
        //     await exec.Command(createUser,
        //         new CreateUserRequest(
        //             Id: UserId.Generate(),
        //             Username: "admin",
        //             Password: "Password",
        //             Email: new EmailAddress("admin@example.com"),
        //             SecurityStamp: Guid.NewGuid().ToString(),
        //             EmailConfirmed: true
        //         ));
        // }
        // catch (Exception) {
        //     // ignored
        // }


        app.Run();
    }
}