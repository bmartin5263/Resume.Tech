using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Repository;

public static class RepositoryUtils {

    public static IDictionary<Type, ISet<Type>> FindRepositoryTypes(string assemblyName) {
        IDictionary<Type, ISet<Type>> result = new Dictionary<Type, ISet<Type>>();
        IDictionary<Type, Type> repoTypes = typeof(IRepository<,>).FindAllKnownGenericSubtypes(assemblyName);
        foreach (var (repoType, interfaceType) in repoTypes) {
            if (result.ContainsKey(repoType)) {
                throw new ConfigurationException("Found duplicate repository types");
            }

            var superTypes = repoType.GetInterfaces().ToHashSet();
            if (repoType.BaseType != null) {
                superTypes.Add(repoType.BaseType);
            }

            result[repoType] = superTypes.Where(t => typeof(object) != t).ToHashSet();
        }

        return result;
    }

}