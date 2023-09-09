namespace ResumeTech.Common.Service; 

public interface IServiceProvider : IScopeProvider {
    public T GetSingleton<T>();
}