namespace ResumeTech.Common.Utility; 

public static class TypeUtils {
    
    public static IEnumerable<Type> FindAllKnownSubtypes(this Type type, string assemblyName) {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName))
            .SelectMany(a => a.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });
    }    
    
    public static IDictionary<Type, Type> FindAllKnownGenericSubtypes(this Type genericType, string assemblyName) {
        var result = new Dictionary<Type, Type>();
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName))
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false });
        foreach (var type in types) {
            var reifiedGenericType = type.GetInheritedGenericType(genericType);
            if (reifiedGenericType != null) {
                result[type] = reifiedGenericType;
            }
        }

        return result;
    }

    public static Type? GetInheritedGenericType(this Type type, Type genericType) {
        if (type.BaseType is not null
            && type.BaseType.IsGenericType
            && type.BaseType.GetGenericTypeDefinition() == genericType) {
            return type.BaseType;
        }
        else {
            return type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);
        }
    }
    
}