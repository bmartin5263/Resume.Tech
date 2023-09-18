using ResumeTech.Common.Events;

namespace ResumeTech.Cqs; 

public interface IUnitOfWork {
    public object DbContext { get; }
    public void RaiseEvent(IDomainEvent domainEvent);
    public Task<ICollection<IDomainEvent>> Commit();
}