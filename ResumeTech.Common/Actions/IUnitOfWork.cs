using ResumeTech.Common.Events;

namespace ResumeTech.Common.Actions; 

public interface IUnitOfWork {
    Task<ICollection<IDomainEvent>> Commit();
    void RaiseEvent(IDomainEvent domainEvent);
    T GetService<T>() where T : notnull;
    object GetService(Type type);
}