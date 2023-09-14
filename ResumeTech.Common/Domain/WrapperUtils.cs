using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Domain;

public static class WrapperUtils {
    
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