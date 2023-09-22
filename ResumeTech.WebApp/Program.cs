using System.Reflection;
using Microsoft.Extensions.Logging.Console;
using ResumeTech.Application.Middleware;
using ResumeTech.Application.Serialization;
using ResumeTech.Application.Util;
using ResumeTech.Common.Options;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application;

internal class Program {
    public const string RootAssembly = "ResumeTech";

    protected Program() {
        
    }
    
    public static void Main(string[] args) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Identities"));
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Experiences"));

        var builder = WebApplication.CreateBuilder(args);
        var userOptions = builder.BindOptions<UserOptions>("User");
        var securityOptions = builder.BindOptions<SecurityOptions>("Security");
        var databaseOptions = builder.BindOptions<DatabaseOptions>("Database");
        var mappedTypes = TypeMapping.GenerateTypeMappings();

        builder.ConfigureJson(mappedTypes);
        builder.ConfigureDatabase(databaseOptions);
        builder.ConfigureSwagger(mappedTypes);
        builder.ConfigureAuthentication(userOptions, securityOptions);
        builder.ConfigureCors();

        builder.AutoAddServices();
        builder.ManuallyAddServices();

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

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<SetUserMiddleware>();
        app.UseExceptionHandler(new ExceptionHandlerOptions {
            ExceptionHandler = new JsonExceptionMiddleware(app.Environment.IsEnvironment("Local")).Invoke
        });
        app.UseAuthorization();
        app.MapControllers();

        Logging.LoggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        if (builder.Environment.EnvironmentName != "Test") {
            app.MigrateDb();
        }
        
        /*
         * exec.Authorize<PatchJobCommand>(RoleName.Hello)
         */

        app.Run();
    }
}