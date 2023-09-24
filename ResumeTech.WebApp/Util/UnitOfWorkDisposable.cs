using ResumeTech.Common.Actions;
using ResumeTech.Common.Events;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Util; 

public sealed class UnitOfWorkDisposable : IUnitOfWorkDisposable {
    private static readonly ILogger Log = Logging.CreateLogger<UnitOfWorkDisposable>();

    private IServiceScope Scope { get; }
    private IUnitOfWork UnitOfWork { get; }
    private bool Disposed { get; set; }

    public UnitOfWorkDisposable(IServiceScope scope, IUnitOfWork unitOfWork) {
        Scope = scope;
        UnitOfWork = unitOfWork;
    }

    public Task<ICollection<IDomainEvent>> Commit() {
        if (Disposed) {
            throw new AppException("Cannot Commit a disposed Unit of Work");
        }
        return UnitOfWork.Commit();
    }

    public void RaiseEvent(IDomainEvent domainEvent) {
        if (Disposed) {
            throw new AppException("Cannot Raise Event on a disposed Unit of Work");
        }
        UnitOfWork.RaiseEvent(domainEvent);
    }

    public T GetService<T>() where T : notnull {
        if (Disposed) {
            throw new AppException("Cannot get service on a disposed Unit of Work");
        }
        return UnitOfWork.GetService<T>();
    }

    public object GetService(Type type) {
        if (Disposed) {
            throw new AppException("Cannot get service on a disposed Unit of Work");
        }
        return UnitOfWork.GetService(type);
    }

    public void Dispose() {
        Scope.Dispose();
        Disposed = true;
    }
}