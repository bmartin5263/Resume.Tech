using System.Text.Json;
using System.Text.Json.Serialization;
using ResumeTech.Common.Domain;

namespace ResumeTech.Application.Serialization.Converters; 

public abstract class WrapperConverter<T, V> : JsonConverter<T> where T : IWrapper<V> where V : notnull {
    
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value.Value, options);
    }
}

public class StringWrapperConverter<T> : WrapperConverter<T, string> where T : IWrapper<string> {
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return (T?) Activator.CreateInstance(typeof(T), reader.GetString()!);
    }
}

public class GuidWrapperConverter<T> : WrapperConverter<T, Guid> where T : IWrapper<Guid> {
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();
        return str == null ? default : (T) Activator.CreateInstance(typeof(T), args: Guid.Parse(str))!;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value.Value.ToString("N"), options);
    }
}

public class GenericJsonStringConverter<T> : JsonConverter<T> where T : notnull {
    private Func<T, string> ToStr { get; }
    private Func<string, T> FromStr { get; }

    public GenericJsonStringConverter(Func<string, T> FromStr) {
        this.ToStr = obj => obj.ToString()!;
        this.FromStr = FromStr;
    }
    
    public GenericJsonStringConverter(Func<T, string> ToStr, Func<string, T> FromStr) {
        this.ToStr = ToStr;
        this.FromStr = FromStr;
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();
        return str == null ? default : FromStr(str);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        writer.WriteStringValue(ToStr(value));
    }
    
}