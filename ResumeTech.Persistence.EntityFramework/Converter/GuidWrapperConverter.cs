using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ResumeTech.Common.Domain;

namespace ResumeTech.Persistence.EntityFramework.Converter; 

public class GuidWrapperConverter<T> : ValueConverter<T, Guid> where T : IWrapper<Guid> {
    public GuidWrapperConverter(
    ) : base(
        convertToProviderExpression: v => v.Value, 
        convertFromProviderExpression: v => (T) Activator.CreateInstance(typeof(T), v)!
    ) { }
}

public class WrapperConverter<T, V> : ValueConverter<T, V> where T : IWrapper<V> where V : notnull {
    public WrapperConverter(
    ) : base(
        convertToProviderExpression: v => v.Value, 
        convertFromProviderExpression: v => (T) Activator.CreateInstance(typeof(T), v)!
    ) { }
}