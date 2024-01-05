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

    public static bool IsEmpty<T>(this IEnumerable<T>? enumerable) {
        return enumerable == null || !enumerable.Any();
    }

    public static bool IsNotEmpty<T>(this IEnumerable<T>? enumerable) {
        return !enumerable.IsEmpty();
    }

    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? enumerable) {
        return enumerable ?? Enumerable.Empty<T>();
    }
}