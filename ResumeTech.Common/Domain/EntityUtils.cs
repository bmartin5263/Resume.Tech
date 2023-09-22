namespace ResumeTech.Common.Domain;

public static class EntityUtils {
    
    public static bool IsDeleted<T, ID>(this T obj) where T : ISoftDeletable {
        return obj.IsDeleted;
    }
    
    public static IDictionary<Type, Type> FindSingleValueEntityIdTypes(string assemblyName) {
        var result = new Dictionary<Type, Type>();
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName!.StartsWith(assemblyName))
            .SelectMany(a => a.GetTypes())
            .Where(t => t is { IsAbstract: false, IsInterface: false });
        
        foreach (var type in types) {
            if (typeof(IEntityId).IsAssignableFrom(type)) {
                Console.WriteLine($"Found Entity Id Type {type}");
            }
        }
        
        return result;
    }
}