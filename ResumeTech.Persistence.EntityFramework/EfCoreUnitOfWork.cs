using Microsoft.EntityFrameworkCore;
using NLog;
using ResumeTech.Common.Events;
using ResumeTech.Cqs;

namespace ResumeTech.Persistence.EntityFramework; 

public sealed class EfCoreUnitOfWork : IUnitOfWork {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public object DbContext { get; }
    private ICollection<IDomainEvent> Events { get; }
    public EFCoreContext EfCoreContext => (DbContext as EFCoreContext)!;

    public EfCoreUnitOfWork(EFCoreContext appDb) {
        DbContext = appDb;
        Events = new List<IDomainEvent>();
    }

    public Task SaveDbChanges() {
        return EfCoreContext.SaveChangesAsync();
    }

    public void RaiseEvent(IDomainEvent domainEvent) {
        Events.Add(domainEvent);
    }

    public async Task<ICollection<IDomainEvent>> Commit() {
        await EfCoreContext.SaveChangesAsync();
        var events = FindDomainEvents();
        ResetContext(); // Allows the same unit of work instance to be reused
        return events;
    }

    private IList<IDomainEvent> FindDomainEvents() {
        var events = new List<IDomainEvent>(Events);
        foreach (var entry in EfCoreContext.ChangeTracker.Entries()) {
            // if (entry.State is EntityState.Added or EntityState.Modified && entry.Entity is IEventPublisher eventPublisher) {
            //     events.AddRange(eventPublisher.DomainEvents);
            // }
        }
        return events;
    }

    private void ResetContext() {
        EfCoreContext.ChangeTracker.Clear();
        Events.Clear();
    }
}