namespace ResumeTech.Common.Service; 

public interface IScope : IDisposable {
    public T GetService<T>() where T : notnull;
    public IScope CreateScope();
}