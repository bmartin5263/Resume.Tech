using ResumeTech.Common.Events;

namespace ResumeTech.Common.Cqs; 

public interface IUnitOfWork {
    public object DbContext { get; }
    public Task SaveDbChanges();
    public void RaiseEvent(IDomainEvent domainEvent);
    public ICollection<IDomainEvent> Commit();
}