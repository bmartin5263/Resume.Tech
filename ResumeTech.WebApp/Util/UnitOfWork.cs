using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Error;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;
using ResumeTech.Persistence.EntityFramework;

namespace ResumeTech.Application.Util; 

public sealed class UnitOfWork : IUnitOfWork {
    private static readonly ILogger Log = Logging.CreateLogger<UnitOfWorkDisposable>();

    private EFCoreContext DbContext { get; }
    private ICollection<IDomainEvent> Events { get; }
    private IServiceProvider ServiceProvider { get; }
    private bool Committed { get; set; }

    public UnitOfWork(IServiceProvider serviceProvider) {
        Console.WriteLine("New Unit of Work");
        DbContext = serviceProvider.GetRequiredService<EFCoreContext>();
        Events = new List<IDomainEvent>();
        ServiceProvider = serviceProvider;
    }

    public void RaiseEvent(IDomainEvent domainEvent) {
        if (Committed) {
            throw new AppException("Cannot RaiseEvent on a committed Unit of Work");
        }
        Events.Add(domainEvent);
    }

    public T GetService<T>() where T : notnull {
        return ServiceProvider.GetRequiredService<T>();
    }

    public object GetService(Type type) {
        return ServiceProvider.GetRequiredService(type);
    }

    public async Task<ICollection<IDomainEvent>> Commit() {
        if (Committed) {
            return Events;
        }
        
        await DbContext.SaveChangesAsync();
        foreach (var entry in DbContext.ChangeTracker.Entries()) {
            if (entry.State is EntityState.Added or EntityState.Modified && entry.Entity is IEventPublisher eventPublisher) {
                foreach (var @event in eventPublisher.DomainEvents) {
                    Events.Add(@event);
                }
            }
        }

        Committed = true;
        return Events;
    }
}