using Microsoft.OpenApi.Models;
using ResumeTech.Application.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ResumeTech.Application; 

public static class SwaggerUtils {
    
    private static Dictionary<JsonType, string> JsonTypeToName { get; } = new() {
        { JsonType.String, "string" },
        { JsonType.Guid, "string" },
        { JsonType.Number, "number" },
        { JsonType.Boolean, "boolean" }
    };

    public static void ConfigureSwagger(this WebApplicationBuilder builder, IList<TypeMapping> typeMappings) {
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
    
    public static void UnwrapWrappedTypes(this SwaggerGenOptions options, IList<TypeMapping> typeMappings) {
        foreach (var typeMapping in typeMappings) {
            options.MapType(typeMapping.Source, () => new OpenApiSchema {
                Type = JsonTypeToName[typeMapping.JsonType],
                Example = typeMapping.ExampleValue
            });
        }
    }
}