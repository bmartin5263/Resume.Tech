using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Events;

namespace ResumeTech.TestUtil; 

public sealed class UnitOfWorkMock : IUnitOfWorkDisposable {
    public IList<IDomainEvent> Events { get; } = new List<IDomainEvent>();
    public UserDetails User => GetService<IUserProvider>().CurrentUser;
    public Dictionary<Type, object> Services { get; } = new();
    public bool Disposed { get; private set; }

    public UnitOfWorkMock(UserDetails? user = null, IDictionary<Type, object>? services = null) {
        if (services != null) {
            foreach (KeyValuePair<Type,object> entry in services) {
                Services.Add(entry.Key, entry.Value);
            }
        }

        if (!Services.ContainsKey(typeof(IUserProvider))) {
            Services[typeof(IUserProvider)] = new UserProviderMock(user ?? UserDetails.NotLoggedIn);
        }
        
        if (!Services.ContainsKey(typeof(IExec))) {
            Services[typeof(IExec)] = new ExecMock();
        }
    }

    public void Login(UserDetails user) {
        GetService<IUserProvider>().Login(user);
    }
    
    public void RaiseEvent(IDomainEvent domainEvent) {
        if (Disposed) {
            throw new AppException("Disposed");
        }
        Events.Add(domainEvent);
    }

    public T GetService<T>() where T : notnull {
        if (Disposed) {
            throw new AppException("Disposed");
        }
        return (T) GetService(typeof(T));
    }

    public object GetService(Type type) {
        if (Disposed) {
            throw new AppException("Disposed");
        }
        return Services[type];
    }

    public IUnitOfWorkDisposable New(UserDetails? user = null) {
        if (Disposed) {
            throw new AppException("Disposed");
        }

        var currentUser = GetService<IUserProvider>().CurrentUser;
        if (Services.TryGetValue(typeof(IUnitOfWorkFactory), out var obj)) {
            var factory = (IUnitOfWorkFactory) obj;
            return factory.Create(user ?? currentUser);
        }
        else {
            return new UnitOfWorkMock(user ?? currentUser, Services);
        }
    }

    public Exec Execute() {
        return GetService<Exec>();
    }

    public Task Commit() {
        return Task.CompletedTask;
    }

    public void Dispose() {
        Disposed = true;
    }
}