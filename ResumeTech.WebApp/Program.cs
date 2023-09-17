using System.Reflection;
using ResumeTech.Application.Middleware;
using ResumeTech.Application.Serialization;

namespace ResumeTech.Application;

internal class Program {
    public const string RootAssembly = "ResumeTech";

    protected Program() {
        
    }
    
    public static void Main(string[] args) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Identities"));
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Experiences"));

        var builder = WebApplication.CreateBuilder(args);
        var mappedTypes = TypeMapping.GenerateTypeMappings();

        builder.ConfigureJson(mappedTypes);
        builder.ConfigureSwagger(mappedTypes);

        builder.AutoAddServices();
        builder.ManuallyAddServices();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<SetUserMiddleware>();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}