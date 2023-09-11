using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Json;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Serialization; 

public class DefaultJsonParser : IJsonParser {

    public JsonSerializerOptions Options { get; }

    public DefaultJsonParser(JsonSerializerOptions options) {
        Options = options;
    }

    public T? Read<T>(string json) {
        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public T ReadOrElse<T>(string json, T defaultValue) {
        return JsonSerializer.Deserialize<T>(json, Options) ?? defaultValue;
    }

    public string Write(object? obj) {
        return obj == null ? "null" : JsonSerializer.Serialize(obj, Options);
    }
}