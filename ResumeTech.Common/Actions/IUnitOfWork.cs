using ResumeTech.Common.Auth;
using ResumeTech.Common.Events;

namespace ResumeTech.Common.Actions; 

public interface IUnitOfWork {
    IList<IDomainEvent> Events { get; }
    
    UserDetails User { get; }
    
    void RaiseEvent(IDomainEvent domainEvent);

    T GetService<T>() where T : notnull;

    object GetService(Type type);

    IUnitOfWorkDisposable New(UserDetails? user = null);

    Exec Execute();

    Task Commit();
}