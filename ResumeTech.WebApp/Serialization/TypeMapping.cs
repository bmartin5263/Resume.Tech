using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using ResumeTech.Application.Serialization.Converters;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Serialization; 

public class TypeMapping {
    public Type Source { get; }
    public JsonType JsonType { get; }
    public IOpenApiAny ExampleValue { get; }
    public JsonConverter? JsonConverter { get; }

    public TypeMapping(Type Source, JsonType JsonType, IOpenApiAny? ExampleValue = null, JsonConverter? JsonConverter = null) {
        this.Source = Source;
        this.JsonType = JsonType;
        this.ExampleValue = ExampleValue ?? DefaultExampleValues[JsonType]();
        this.JsonConverter = JsonConverter;
    }

    public static Dictionary<JsonType, Func<IOpenApiAny>> DefaultExampleValues { get; } = new() {
        { JsonType.String, () => new OpenApiString("string") },
        { JsonType.Number, () => new OpenApiInteger(0) },
        { JsonType.Boolean, () => new OpenApiBoolean(false) },
        { JsonType.Guid, () => new OpenApiString(Guid.NewGuid().ToString("N")) }
    };
    
    public static Dictionary<Type, Func<object, IOpenApiAny>> CreateOpenApiValue { get; } = new() {
        { typeof(string), v => new OpenApiString((string) v) },
        { typeof(Guid), v => new OpenApiString(((Guid) v).ToString("N")) },        
        { typeof(int), v => new OpenApiInteger((int) v) },
        { typeof(uint), v => new OpenApiInteger((int) v) },
        { typeof(long), v => new OpenApiLong((long) v) },
        { typeof(ulong), v => new OpenApiLong((long) v) },
        { typeof(float), v => new OpenApiFloat((float) v) },
        { typeof(double), v => new OpenApiDouble((double) v) },
        { typeof(decimal), v => new OpenApiDouble((double) v) },
        { typeof(bool), v => new OpenApiBoolean((bool) v) }
    };

    public static Dictionary<Type, JsonType> JsonTypeByCsharpType { get; } = new() {
        { typeof(string), JsonType.String },
        { typeof(Guid), JsonType.Guid },        
        { typeof(int), JsonType.Number },
        { typeof(uint), JsonType.Number },
        { typeof(long), JsonType.Number },
        { typeof(ulong), JsonType.Number },
        { typeof(float), JsonType.Number },
        { typeof(double), JsonType.Number },
        { typeof(decimal), JsonType.Number },
        { typeof(bool), JsonType.Boolean },
    };

    public static Dictionary<Type, Type> WrapperConverters { get; } = new() {
        { typeof(string), typeof(StringWrapperConverter<>) },
        { typeof(Guid), typeof(GuidWrapperConverter<>) },
    };

    public static IList<TypeMapping> GenerateTypeMappings() {
        // Add static mappings here
        var mappings = new List<TypeMapping> {
            new(
                Source: typeof(DateOnly),
                JsonType: JsonType.String,
                ExampleValue: new OpenApiString(DateOnly.FromDateTime(DateTime.UtcNow).ToString()),
                JsonConverter: new GenericJsonStringConverter<DateOnly>(
                    FromStr: DateOnly.Parse
                )
            )
        };

        // Dynamically generated mappings
        var wrappedTypes = WrapperUtils.FindAllWrappedTypes("ResumeTech");
        foreach (var (wrapper, wrapee) in wrappedTypes) {
            object? exampleValue = wrapper.GetProperty("ExampleValue")?.GetValue(null, null);
            JsonType jsonType = JsonTypeByCsharpType[wrapee];
            Type wrapperConverterType = WrapperConverters[wrapee];
            Type reifiedConverterType = wrapperConverterType.MakeGenericType(wrapper);
            
            mappings.Add(new TypeMapping(
                Source: wrapper,
                JsonType: jsonType,
                ExampleValue: exampleValue != null
                    ? CreateOpenApiValue[wrapee](exampleValue)
                    : DefaultExampleValues[jsonType](),
                JsonConverter: (JsonConverter) Activator
                    .CreateInstance(reifiedConverterType)
                    .OrElseThrow("Failed to create Instance")
            ));
        }
        
        return mappings;
    }
    
}