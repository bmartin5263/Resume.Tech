using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;
using ResumeTech.Identities.Filters;

namespace ResumeTech.Identities.Util; 

public class Authorizer<TEntity> where TEntity : class {
    private IList<IAccessFilter<TEntity>> Filters { get; }
    private UserIdProvider UserIdProvider { get; }
    public UserId CurrentUser => UserIdProvider.Get();

    public Authorizer(IList<IAccessFilter<TEntity>> Filters, UserIdProvider UserIdProvider) {
        this.Filters = Filters;
        this.UserIdProvider = UserIdProvider;
    }

    public async Task<TEntity> AssertCanRead(TEntity entity) {
        if (!await CanRead(entity)) {
            throw new AuthorizationException();
        }
        return entity;
    }
    

    public async Task<bool> CanRead(TEntity entity) {
        var userId = UserIdProvider.Get();
        foreach (var filter in Filters) {
            if (!await filter.CheckCanRead(userId, entity)) {
                return false;
            }
        }
        return true;
    }
}