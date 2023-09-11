namespace ResumeTech.Common.Service; 

public interface IScope : IDisposable {
    public T GetScoped<T>() where T : notnull;
    public object GetScoped(Type type);
}