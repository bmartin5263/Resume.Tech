namespace ResumeTech.Common.Actions; 

public interface IUnitOfWorkFactory {
    IUnitOfWorkDisposable Create();
}