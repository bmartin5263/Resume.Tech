using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using ResumeTech.Application.Serialization;
using ResumeTech.Common.Cqs.Commands;
using ResumeTech.Common.Cqs.Queries;
using ResumeTech.Common.Json;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Filters;
using ResumeTech.Identities.Util;
using Swashbuckle.AspNetCore.SwaggerGen;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace ResumeTech.Application;

internal class Program {
    public const string RootAssembly = "ResumeTech";

    protected Program() {
        
    }
    
    public static void Main(string[] args) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Identities"));

        var builder = WebApplication.CreateBuilder(args);
        var mappedTypes = TypeMapping.GenerateTypeMappings();

        ConfigureJson(builder, mappedTypes);
        ConfigureSwagger(builder, mappedTypes);

        AutoAddServices(builder);
        builder.Services.AddScoped<JobManager>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void AutoAddServices(WebApplicationBuilder builder) {
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

    private static void AutoAddCqsCommands(WebApplicationBuilder builder) {
        var commandTypes = typeof(CqsCommand).FindAllKnownSubtypes(RootAssembly);
        foreach (var commandType in commandTypes) {
            Console.WriteLine($"Adding Command: {commandType}");
            builder.Services.AddScoped(commandType);
        }
    }

    private static void AutoAddCqsQueries(WebApplicationBuilder builder) {
        var queryTypes = typeof(CqsQuery).FindAllKnownSubtypes(RootAssembly);
        foreach (var queryType in queryTypes) {
            Console.WriteLine($"Adding Query: {queryType}");
            builder.Services.AddScoped(queryType);
        }
    }

    private static void AddInMemoryRepositories(WebApplicationBuilder builder) {
        AppDomain.CurrentDomain.Load(new AssemblyName("ResumeTech.Persistence.InMemory"));
        IDictionary<Type, ISet<Type>> repoTypes = RepositoryUtils.FindRepositoryTypes(RootAssembly);
        foreach (var (repoType, interfaceTypes) in repoTypes) {
            foreach (var interfaceType in interfaceTypes) {
                Console.WriteLine($"Adding Repository with Interface: {repoType.Name}, {interfaceType.Name}");
                builder.Services.AddSingleton(interfaceType, repoType);
            }
        }
    }
    
    private static void ConfigureJson(WebApplicationBuilder builder, IList<TypeMapping> typeMappings) {
        // Apply custom json settings to controllers
        IJsonParser.Default = new DefaultJsonParser(CreateDefaultOptions(typeMappings));
        builder.Services.Configure<JsonOptions>(o => o.CopyOptionsFrom(IJsonParser.Default.Options));
        builder.Services.Configure<MvcJsonOptions>(o => o.CopyOptionsFrom(IJsonParser.Default.Options));
    }
    
    private static JsonSerializerOptions CreateDefaultOptions(IList<TypeMapping> typeMappings) {
        JsonSerializerOptions options = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            // TypeInfoResolver = new PrivateConstructorContractResolver(),
            Converters = {
                new JsonStringEnumConverter()
            }
        };

        var customConverters = typeMappings
            .Select(t => t.JsonConverter)
            .Where(t => t != null);

        foreach (var converter in customConverters) {
            options.Converters.Add(converter!);
        }

        return options;
    }
    
    private static void ConfigureSwagger(WebApplicationBuilder builder, IList<TypeMapping> typeMappings) {
        builder.Services.AddSwaggerGen(options => {
            options.UnwrapWrappedTypes(typeMappings); // Unwraps any types that would be like { "value": 33 } into just 33
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
    
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}

public static class ProgramExtensions {

    private static Dictionary<JsonType, string> JsonTypeToName { get; } = new() {
        { JsonType.String, "string" },
        { JsonType.Guid, "string" },
        { JsonType.Number, "number" },
        { JsonType.Boolean, "boolean" }
    };

    public static void UnwrapWrappedTypes(this SwaggerGenOptions options, IList<TypeMapping> typeMappings) {
        foreach (var typeMapping in typeMappings) {
            options.MapType(typeMapping.Source, () => new OpenApiSchema {
                Type = JsonTypeToName[typeMapping.JsonType],
                Example = typeMapping.ExampleValue
            });
        }
    }
    
    public static void AddJsonConverter(this JsonOptions options, JsonConverter converter) {
        options.SerializerOptions.Converters.Add(converter);
    }

    public static void AddJsonConverter(this MvcJsonOptions options, JsonConverter converter) {
        options.JsonSerializerOptions.Converters.Add(converter);
    }

    public static void CopyOptionsFrom(this JsonOptions self, JsonSerializerOptions options) {
        foreach (var jsonConverter in options.Converters) {
            self.AddJsonConverter(jsonConverter);
        }
        self.SerializerOptions.TypeInfoResolver = options.TypeInfoResolver;
        self.SerializerOptions.PropertyNamingPolicy = options.PropertyNamingPolicy;
    }

    public static void CopyOptionsFrom(this MvcJsonOptions self, JsonSerializerOptions options) {
        foreach (var jsonConverter in options.Converters) {
            self.AddJsonConverter(jsonConverter);
        }
        self.JsonSerializerOptions.TypeInfoResolver = options.TypeInfoResolver;
        self.JsonSerializerOptions.PropertyNamingPolicy = options.PropertyNamingPolicy;
    }
    
    public static T BindOptions<T>(this WebApplicationBuilder builder, string key) where T : class, new() {
        builder.Services.AddScoped<T>(service => service.GetRequiredService<IConfiguration>().BindOptions<T>(key));
        return builder.Configuration.BindOptions<T>(key);
    }

    public static T BindOptions<T>(this IConfiguration configuration, string key) where T : class, new() {
        var options = new T();
        configuration.GetSection(key).Bind(options);
        return options;
    }
}