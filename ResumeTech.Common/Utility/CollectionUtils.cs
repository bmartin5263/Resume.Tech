using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Utility; 

public static class CollectionUtils {

    public static bool ContainsAny<T>(this T[] roles, IEnumerable<T> other) {
        return other.Any(roles.Contains);
    }

    public static bool IsEmpty<T>(this ICollection<T>? collection) {
        return collection == null || collection.Count == 0;
    }

    public static bool IsNotEmpty<T>(this ICollection<T>? collection) {
        return !collection.IsEmpty();
    }
}