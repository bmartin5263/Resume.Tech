using System.Reflection;
using ResumeTech.Common.Cqs.Commands;
using ResumeTech.Common.Cqs.Queries;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Filters;
using ResumeTech.Identities.Util;

namespace ResumeTech.Application; 

public static class ServiceUtils {
    
    public static void AutoAddServices(this WebApplicationBuilder builder) {
        AutoAddCqsCommands(builder);
        AutoAddCqsQueries(builder);

        builder.Services.AddScoped<UserIdProvider>();
        builder.Services.AddScoped<Authorizer<Job>>(s => new Authorizer<Job>(
            Filters: new List<IAccessFilter<Job>>() {
                new IsOwnerFilter<Job>()
            },
            UserIdProvider: s.GetRequiredService<UserIdProvider>()
        ));
        if (builder.Environment.IsDevelopment()) {
            AddInMemoryRepositories(builder);
        }
        else {
            throw new ArgumentException("Invalid profile");
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

    private static void AddInMemoryRepositories(this WebApplicationBuilder builder) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Persistence.InMemory"));
        IDictionary<Type, ISet<Type>> repoTypes = RepositoryUtils.FindRepositoryTypes(Program.RootAssembly);
        foreach (var (repoType, interfaceTypes) in repoTypes) {
            foreach (var interfaceType in interfaceTypes) {
                Console.WriteLine($"Adding Repository with Interface: {repoType.Name}, {interfaceType.Name}");
                builder.Services.AddSingleton(interfaceType, repoType);
            }
        }
    }
    
    
    public static void ManuallyAddServices(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<JobManager>();
    }
}