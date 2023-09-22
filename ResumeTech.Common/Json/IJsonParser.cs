using System.Text.Json;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Json; 

public interface IJsonParser {
    private static IJsonParser? defaultParser;

    // Used globally across the application, but must be initially set on startup
    public static IJsonParser Default {
        get => defaultParser.OrElseThrow("Default parser has not yet been set");
        set {
            if (defaultParser != null) {
                throw new AppException("Cannot set the default parser more than once");
            }
            defaultParser = value;
        }
    }

    public JsonSerializerOptions Options { get; }
    public T? Read<T>(string json);
    public T ReadOrElse<T>(string json, T defaultValue);
    public string Write(object? obj);
    public Task WriteAsync<T>(Stream stream, T value);
}