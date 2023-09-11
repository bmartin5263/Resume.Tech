using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ResumeTech.Application.Adapters;
using ResumeTech.Application.Serialization;
using ResumeTech.Application.Serialization.Converters;
using ResumeTech.Common.Cqs.Commands;
using ResumeTech.Common.Cqs.Queries;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Json;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using Swashbuckle.AspNetCore.SwaggerGen;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace ResumeTech.Application;

internal class Program {

    protected Program() {
        
    }
    
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        var mappedTypes = TypeMapping.GenerateTypeMappings();

        ConfigureJson(builder, mappedTypes);
        ConfigureSwagger(builder, mappedTypes);

        // Add services to the container.
        
        var commandTypes = typeof(CqsCommand).FindAllKnownSubtypes("ResumeTech");
        foreach (var commandType in commandTypes) {
            builder.Services.AddScoped(commandType);
        }
        var queryTypes = typeof(CqsQuery).FindAllKnownSubtypes("ResumeTech");
        foreach (var queryType in queryTypes) {
            builder.Services.AddScoped(queryType);
        }
        
        builder.Services.AddScoped<JobManager>();
        builder.Services.AddSingleton<IJobRepository, JobMemoryRepository>();

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