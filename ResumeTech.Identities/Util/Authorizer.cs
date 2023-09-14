using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;
using ResumeTech.Identities.Filters;

namespace ResumeTech.Identities.Util; 

public class Authorizer<TEntity> {
    private IList<IAccessFilter<TEntity>> Filters { get; }
    private UserIdProvider UserIdProvider { get; }

    public Authorizer(IList<IAccessFilter<TEntity>> Filters, UserIdProvider UserIdProvider) {
        this.Filters = Filters;
        this.UserIdProvider = UserIdProvider;
    }

    public TEntity AssertCanRead(TEntity entity) {
        if (!CanRead(entity)) {
            throw new AuthorizationException();
        }
        return entity;
    }
    
    public TEntity AssertCanWrite(TEntity entity) {
        if (!CanWrite(entity)) {
            throw new AuthorizationException();
        }
        return entity;
    }
    
    public bool CanRead(TEntity entity) {
        var userId = UserIdProvider.Get();
        return Filters.All(filter => filter.CheckCanRead(userId, entity));
    }
    
    public bool CanWrite(TEntity entity) {
        var userId = UserIdProvider.Get();
        return Filters.All(filter => filter.CheckCanWrite(userId, entity));
    }
    
    public IEnumerable<TEntity> FilterUnreadableEntities(IEnumerable<TEntity> entities) {        
        var userId = UserIdProvider.Get();
        return entities
            .Where(e => Filters.All(filter => filter.CheckCanRead(userId, e)));
    }
    
    public IEnumerable<TEntity> FilterUnwritableEntities(IEnumerable<TEntity> entities) {        
        var userId = UserIdProvider.Get();
        return entities
            .Where(e => Filters.All(filter => filter.CheckCanWrite(userId, e)));
    }
}

public static class AuthorizationExtensions {

    public static TEntity AssertCanRead<TEntity>(this TEntity entity, Authorizer<TEntity> authorizer) {
        return authorizer.AssertCanRead(entity);
    }
    
    public static TEntity AssertCanWrite<TEntity>(this TEntity entity, Authorizer<TEntity> authorizer) {
        return authorizer.AssertCanWrite(entity);
    }
    
}