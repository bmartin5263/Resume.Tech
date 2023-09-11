using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Domain;

/**
 * Interface representing classes who's only purpose is to wrap another non-null object, with the
 * intention of giving it additional type-safety. One example is wrapping a plain `string` in a
 * `RoutingNumber` object to provide additional type information while retaining the `string` as
 * the implementation
 *
 * The purpose of implementing this interface is that some app configuration automatically scans for
 * classes implementing `IWrapper` and can perform some special function with them. Such as when writing
 * json responses we can omit the wrapper object and instead inline `Value`.
 */
public interface IWrapper<out T> where T : notnull {
    T Value { get; }
}

public class WrapperUtils {
    
    public static IDictionary<Type, Type> FindAllWrappedTypes(string assemblyName) {
        var result = new Dictionary<Type, Type>();
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName))
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false });
        
        foreach (var type in types) {
            var reifiedGenericType = type.GetInheritedGenericType(typeof(IWrapper<>));
            if (reifiedGenericType != null) {
                result[type] = reifiedGenericType.GetGenericArguments()[0].OrElseThrow($"Missing wrapped type for {reifiedGenericType}");
            }
        }
        
        return result;
    }
    
}