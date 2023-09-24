using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Utility; 

public static class CollectionUtils {

    public static bool ContainsAny<T>(this T[] roles, IEnumerable<T> other) {
        return other.Any(roles.Contains);
    }
    
}