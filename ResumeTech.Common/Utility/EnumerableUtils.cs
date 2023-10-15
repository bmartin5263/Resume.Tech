using System.Collections.Immutable;
using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Utility; 

public static class EnumerableUtils {

    public static bool ContainsAny<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs) {
        var set = lhs.ToImmutableHashSet();
        return rhs.Any(set.Contains);
    }
    
    public static bool ContainsAll<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs) {
        var set = lhs.ToImmutableHashSet();
        return rhs.All(set.Contains);
    }

    public static bool ContainsNone<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs) {
        return !ContainsAny(lhs, rhs);
    }

    public static bool IsEmpty<T>(this IEnumerable<T>? collection) {
        return collection == null || !collection.Any();
    }

    public static bool IsNotEmpty<T>(this IEnumerable<T>? collection) {
        return !collection.IsEmpty();
    }
}