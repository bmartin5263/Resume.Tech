using System.Text;

namespace ResumeTech.Common.Utility; 

public static class StringUtils {

    public static bool IsBlank(this string? str) {
        return str == null || str.Trim().Length == 0;
    }

    public static string ToExpandedString<T>(this IEnumerable<T>? enumerable) {
        if (enumerable == null) {
            return "[]";
        }
        var sb = new StringBuilder("[");
        using var enumerator = enumerable.GetEnumerator();
        
        if (enumerator.MoveNext()) {
            sb.Append(enumerator.Current);
        }
        while (enumerator.MoveNext()) {
            sb.Append(", ").Append(enumerator.Current);
        }

        sb.Append(']');
        return sb.ToString();
    }
    
    public static string ToExpandedString<K, V>(this IDictionary<K, V>? enumerable) {
        if (enumerable == null) {
            return "{}";
        }
        var sb = new StringBuilder("{");
        using var enumerator = enumerable.GetEnumerator();
        
        if (enumerator.MoveNext()) {
            sb.Append(enumerator.Current.Key).Append(": ").Append(enumerator.Current.Value);
        }
        while (enumerator.MoveNext()) {
            sb.Append(", ");
            sb.Append(enumerator.Current.Key).Append(": ").Append(enumerator.Current.Value);
        }

        sb.Append('}');
        return sb.ToString();
    }
    
}