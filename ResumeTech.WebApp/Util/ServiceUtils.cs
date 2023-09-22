using ResumeTech.Common.Auth;
using ResumeTech.Common.Cqs;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Auth.Filters;

namespace ResumeTech.Application.Util; 

public static class ServiceUtils {
    
    public static void AutoAddServices(this WebApplicationBuilder builder) {
        builder.AutoAddCqsCommands();
        builder.AutoAddCqsQueries();
        
        if (builder.Environment.EnvironmentName == "Test") {
            builder.AddInMemoryRepositories();
        }
        else {
            builder.AddRepositories();
        }
    }

    private static void AutoAddCqsCommands(this WebApplicationBuilder builder) {
        var commandTypes = typeof(CqsCommand).FindAllKnownSubtypes(Program.RootAssembly);
        foreach (var commandType in commandTypes) {
            Console.WriteLine($"Adding Command: {commandType}");
            builder.Services.AddScoped(commandType);
        }
    }

    private static void AutoAddCqsQueries(this WebApplicationBuilder builder) {
        var queryTypes = typeof(CqsQuery).FindAllKnownSubtypes(Program.RootAssembly);
        foreach (var queryType in queryTypes) {
            Console.WriteLine($"Adding Query: {queryType}");
            builder.Services.AddScoped(queryType);
        }
    }

    public static void ManuallyAddServices(this WebApplicationBuilder builder) {
        builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
        
        builder.Services.AddScoped<JobManager>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<Exec>();
        builder.Services.AddScoped<IUserDetailsProvider, UserDetailsProvider>();
        builder.Services.AddScoped<Authorizer<Job>>(s => new Authorizer<Job>(
            Filters: new List<IAccessFilter<Job>> {
                new IsOwnerFilter<Job>()
            },
            UserDetailsProvider: s.GetRequiredService<IUserDetailsProvider>()
        ));
    }
}