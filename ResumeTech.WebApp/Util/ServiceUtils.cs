using System.Collections.Immutable;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Cqs;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Cqs;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Auth.Filters;
using ResumeTech.Identities.Duende;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Util; 

public static class ServiceUtils {
    
    public static IList<Type> AutoAddServices(this WebApplicationBuilder builder) {
        var commandsAndQueries = new List<Type>();
        
        commandsAndQueries.AddRange(builder.AutoAddCqsCommands());
        commandsAndQueries.AddRange(builder.AutoAddCqsQueries());
        
        if (builder.Environment.EnvironmentName == "Test") {
            builder.AddInMemoryRepositories();
        }
        else {
            builder.AddRepositories();
        }

        return commandsAndQueries;
    }

    private static IList<Type> AutoAddCqsCommands(this WebApplicationBuilder builder) {
        var commandTypes = typeof(CqsCommand).FindAllKnownSubtypes(Program.RootAssembly).ToImmutableList();
        foreach (var commandType in commandTypes) {
            builder.Services.AddScoped(commandType);
        }
        return commandTypes;
    }

    private static IList<Type> AutoAddCqsQueries(this WebApplicationBuilder builder) {
        var queryTypes = typeof(CqsQuery).FindAllKnownSubtypes(Program.RootAssembly).ToImmutableList();
        foreach (var queryType in queryTypes) {
            builder.Services.AddScoped(queryType);
        }
        return queryTypes;
    }

    public static void ManuallyAddServices(this WebApplicationBuilder builder, IEnumerable<Type> commandsAndQueries) {
        builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
        
        builder.Services.AddScoped<JobManager>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<Exec>();
        builder.Services.AddScoped<IUserDetailsProvider, UserDetailsProvider>();
        builder.Services.AddScoped<IUserManager, DuendeUserManager>();
        builder.Services.AddScoped<Authorizer<Job>>(s => new Authorizer<Job>(
            Filters: new List<IAccessFilter<Job>> {
                new IsOwnerFilter<Job>()
            },
            UserDetailsProvider: s.GetRequiredService<IUserDetailsProvider>()
        ));

        foreach (var action in commandsAndQueries) {
            builder.Services.AddScoped(action.BaseType.OrElseThrow("Missing base type for action"), action);
        }
    }
}