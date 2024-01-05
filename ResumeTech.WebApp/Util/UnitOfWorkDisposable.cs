using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Util; 

public sealed class UnitOfWorkDisposable : IUnitOfWorkDisposable {
    private static readonly ILogger Log = Logging.CreateLogger<UnitOfWorkDisposable>();

    private IServiceScope Scope { get; }
    private IUnitOfWork UnitOfWork { get; }
    public IList<IDomainEvent> Events => UnitOfWork.Events;
    public UserDetails User => UnitOfWork.User;
    private bool Disposed { get; set; }

    public UnitOfWorkDisposable(IServiceScope scope, IUnitOfWork unitOfWork) {
        Scope = scope;
        UnitOfWork = unitOfWork;
    }


    public IUnitOfWorkDisposable New(UserDetails? user = null) {
        if (Disposed) {
            throw new InvalidOperationException("Cannot Create a new Unit of Work from a disposed Unit of Work");
        }
        return UnitOfWork.New(user);
    }
    
    public Exec Execute() {
        if (Disposed) {
            throw new InvalidOperationException("Cannot call Execute on a disposed Unit of Work");
        }
        return UnitOfWork.Execute();
    }

    public Task Commit() {
        if (Disposed) {
            throw new InvalidOperationException("Cannot Commit a disposed Unit of Work");
        }
        return UnitOfWork.Commit();
    }

    public void RaiseEvent(IDomainEvent domainEvent) {
        if (Disposed) {
            throw new InvalidOperationException("Cannot Raise Event on a disposed Unit of Work");
        }
        UnitOfWork.RaiseEvent(domainEvent);
    }

    public T GetService<T>() where T : notnull {
        if (Disposed) {
            throw new InvalidOperationException("Cannot get service on a disposed Unit of Work");
        }
        return UnitOfWork.GetService<T>();
    }

    public object GetService(Type type) {
        if (Disposed) {
            throw new InvalidOperationException("Cannot get service on a disposed Unit of Work");
        }
        return UnitOfWork.GetService(type);
    }

    public void Dispose() {
        if (Disposed) {
            return;
        }
        Scope.Dispose();
        Disposed = true;
    }
}