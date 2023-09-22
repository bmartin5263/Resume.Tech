namespace ResumeTech.Common.Cqs; 

public interface IUnitOfWorkFactory {
    IUnitOfWorkDisposable Create();
}