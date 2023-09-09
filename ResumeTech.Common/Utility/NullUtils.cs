namespace ResumeTech.Common.Utility; 

public static class NullUtils {

    public static T OrElseThrow<T>(this T? self, Func<Exception> supplier) {
        if (self == null) {
            throw supplier();
        }
        return self;
    }
    
}