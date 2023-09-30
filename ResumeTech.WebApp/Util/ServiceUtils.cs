using System.Collections.Immutable;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Auth.Filters;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Duende;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Util; 

public static class ServiceUtils {
    
    public static IList<Type> AutoAddServices(this WebApplicationBuilder builder) {
        var commandsAndQueries = new List<Type>();

        builder.AutoAddCqsCommands();
        builder.AutoAddCqsQueries();
        
        if (builder.Environment.EnvironmentName == "Test") {
            builder.AddInMemoryRepositories();
        }
        else {
            builder.AddRepositories();
        }

        return commandsAndQueries;
    }

    private static void AutoAddCqsCommands(this WebApplicationBuilder builder) {
        var commandTypes = typeof(Command<,>).FindAllKnownGenericSubtypesFromBaseClass(Program.RootAssembly).ToImmutableList();
        foreach (var (concreteType, interfaceType) in commandTypes) {
            builder.Services.AddScoped(concreteType);
            builder.Services.AddScoped(interfaceType, concreteType);
        }
    }

    private static void AutoAddCqsQueries(this WebApplicationBuilder builder) {
        var queryTypes = typeof(Query<,>).FindAllKnownGenericSubtypesFromBaseClass(Program.RootAssembly).ToImmutableList();
        foreach (var (concreteType, interfaceType) in queryTypes) {
            builder.Services.AddScoped(concreteType);
            builder.Services.AddScoped(interfaceType, concreteType);
        }
    }

    public static void ManuallyAddServices(this WebApplicationBuilder builder, IEnumerable<Type> commandsAndQueries) {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
        
        builder.Services.AddScoped<JobManager>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IJwtMinter, JwtMinter>();
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