using ResumeTech.Common.Error;

namespace ResumeTech.Common.Utility; 

public static class TypeUtils {
    
    public static IEnumerable<Type> FindAllKnownSubtypes(this Type type, string assemblyName) {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName))
            .SelectMany(a => a.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });
    }    
    
    public static IEnumerable<Type> GetInheritedTypes(this Type type, string assemblyName) {
        var result = new HashSet<Type>(type.GetInterfaces());
        if (type.BaseType != null) {
            result.Add(type.BaseType);
        }
        return result;
    }    
    
    public static IDictionary<Type, Type> FindAllKnownGenericSubtypes(this Type genericType, string assemblyName) {
        var result = new Dictionary<Type, Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName));
        var types = assemblies
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
    
    public static IDictionary<Type, Type> FindAllKnownGenericSubtypesFromBaseClass(this Type genericType, string assemblyName) {
        var result = new Dictionary<Type, Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName));
        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false });
        foreach (var type in types) {
            var reifiedGenericType = type.GetInheritedGenericTypeBaseClass(genericType);
            if (reifiedGenericType != null) {
                result[type] = reifiedGenericType;
            }
        }

        return result;
    }
    
    public static IDictionary<Type, ISet<Type>> FindAllKnownGenericSubtypes2(this Type genericType, string assemblyName) {
        IDictionary<Type, ISet<Type>> result = new Dictionary<Type, ISet<Type>>();
        IDictionary<Type, Type> repoTypes = genericType.FindAllKnownGenericSubtypes(assemblyName);
        foreach (var (repoType, interfaceType) in repoTypes) {
            if (result.ContainsKey(repoType)) {
                throw new AppException("Found duplicate repository types");
            }

            var superTypes = repoType.GetInterfaces().ToHashSet();
            if (repoType.BaseType != null) {
                superTypes.Add(repoType.BaseType);
            }

            result[repoType] = superTypes.Where(t => typeof(object) != t).ToHashSet();
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
    

    public static Type? GetInheritedGenericTypeBaseClass(this Type type, Type genericType) {
        var currentType = type;
        while (currentType.BaseType is not null && currentType.BaseType != typeof(object)) {
            if (currentType.BaseType.IsGenericType && currentType.BaseType.GetGenericTypeDefinition() == genericType) {
                return currentType.BaseType;
            }
            currentType = currentType.BaseType;
        }

        return null;
    }
    
}