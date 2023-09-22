using ResumeTech.Common.Cqs;

namespace ResumeTech.Application.Util; 

public class UnitOfWorkFactory : IUnitOfWorkFactory {
    private IServiceScopeFactory ScopeFactory { get; }
    
    public UnitOfWorkFactory(IServiceScopeFactory scopeFactory) {
        ScopeFactory = scopeFactory;
    }
    
    public IUnitOfWorkDisposable Create() {
        var scope = ScopeFactory.CreateScope();
        return new UnitOfWorkDisposable(scope, new UnitOfWork(scope.ServiceProvider));
    }
}