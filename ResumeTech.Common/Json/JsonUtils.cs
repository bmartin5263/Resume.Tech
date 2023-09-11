namespace ResumeTech.Common.Json; 

public static class JsonUtils {

    public static T? ParseJson<T>(this string json) {
        return IJsonParser.Default.Read<T>(json);
    }
    
    public static T ParseJsonOrElse<T>(this string json, T defaultValue) {
        return IJsonParser.Default.ReadOrElse(json, defaultValue);
    }
    
    public static string ToJson<T>(this T? obj) {
        return IJsonParser.Default.Write(obj);
    }
    
}