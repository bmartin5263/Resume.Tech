using ResumeTech.Common.Error;

namespace ResumeTech.Common.Utility; 

public static class EnumUtils {

    public static T ParseEnumOrThrow<T>(this string self) where T : Enum {
        if (Enum.TryParse(typeof(T), self, out var result)) {
            return (T) result;
        }
        throw new AppException($"Failed to parse enum type {typeof(T)} from string {self}");
    }
    
}