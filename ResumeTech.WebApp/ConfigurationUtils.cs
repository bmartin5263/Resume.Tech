using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using ResumeTech.Application.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ResumeTech.Application;

public static class ConfigurationUtils {

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