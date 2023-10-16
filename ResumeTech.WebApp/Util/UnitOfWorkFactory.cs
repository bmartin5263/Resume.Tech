using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;

namespace ResumeTech.Application.Util; 

public class UnitOfWorkFactory : IUnitOfWorkFactory {
    private IServiceScopeFactory ScopeFactory { get; }
    
    public UnitOfWorkFactory(IServiceScopeFactory scopeFactory) {
        ScopeFactory = scopeFactory;
    }
    
    public IUnitOfWorkDisposable Create(UserDetails userDetails) {
        var scope = ScopeFactory.CreateScope();
        scope.ServiceProvider.GetRequiredService<IUserProvider>().Login(userDetails);
        return new UnitOfWorkDisposable(scope, new UnitOfWork(scope.ServiceProvider));
    }
}