// using ResumeTech.Common.Service;
//
// namespace ResumeTech.Application.Util; 
//
// public class DotNetScope : IScope {
//     private IServiceScope Impl { get; }
//     
//     public DotNetScope(IServiceScope impl) {
//         Impl = impl;
//     }
//
//     public void Dispose() {
//         GC.SuppressFinalize(this);
//         Impl.Dispose();
//     }
//
//     public T GetScoped<T>() where T : notnull {
//         return Impl.ServiceProvider.GetRequiredService<T>();
//     }
//
//     public object GetScoped(Type type) {
//         return Impl.ServiceProvider.GetRequiredService(type);
//     }
// }