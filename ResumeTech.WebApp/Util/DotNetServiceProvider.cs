// using ResumeTech.Common.Service;
//
// namespace ResumeTech.Application.Util; 
//
// public class DotNetServiceProvider : IAppServiceProvider {
//     private System.IServiceProvider Impl { get; }
//     
//     public DotNetServiceProvider(System.IServiceProvider impl) {
//         Impl = impl;
//     }
//
//     public IScope CreateScope() {
//         return new DotNetScope(Impl.CreateScope());
//     }
//
//     public T GetService<T>() where T : notnull {
//         return Impl.GetRequiredService<T>();
//     }
//     
// }