using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Options;
using ResumeTech.Common.Repository;
using ResumeTech.Persistence.EntityFramework;

namespace ResumeTech.Application.Util; 

public static class DatabaseUtils {
    
    public static void ConfigureDatabase(this WebApplicationBuilder builder, DatabaseOptions dbOptions) {
        builder.Services.AddDbContext<EFCoreContext>(options =>
            options.UseNpgsql(dbOptions.ToConnectionString())
        );
    }
    
    public static void AddRepositories(this WebApplicationBuilder builder) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Persistence.EntityFramework"));
        IDictionary<Type, ISet<Type>> repoTypes = RepositoryUtils.FindRepositoryTypes(Program.RootAssembly);
        foreach (var (repoType, interfaceTypes) in repoTypes) {
            foreach (var interfaceType in interfaceTypes) {
                Console.WriteLine($"Adding Repository with Interface: {repoType.Name}, {interfaceType.Name}");
                builder.Services.AddScoped(interfaceType, repoType);
            }
        }
    }
    
    public static void AddInMemoryRepositories(this WebApplicationBuilder builder) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Persistence.InMemory"));
        IDictionary<Type, ISet<Type>> repoTypes = RepositoryUtils.FindRepositoryTypes(Program.RootAssembly);
        foreach (var (repoType, interfaceTypes) in repoTypes) {
            foreach (var interfaceType in interfaceTypes) {
                Console.WriteLine($"Adding Repository with Interface: {repoType.Name}, {interfaceType.Name}");
                builder.Services.AddSingleton(interfaceType, repoType);
            }
        }
    }

    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<EFCoreContext>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Count == 0) {
            Console.WriteLine("Database is up-to-date");
            return;
        }
        Console.WriteLine("Applying Database Migrations:");
        foreach (var migration in pendingMigrations) {
            Console.WriteLine(migration);
        }
        dbContext.Database.Migrate();
    }
}