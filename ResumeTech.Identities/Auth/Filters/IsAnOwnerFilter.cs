using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Auth.Filters; 

public class IsAnOwnerFilter<TEntity> : IAccessFilter<TEntity> where TEntity : class, IMultiOwnedEntity {
    
    public Task<bool> CheckCanRead(UserId userId, TEntity entity) {
        return Task.FromResult(entity.OwnerIds.Contains(userId));
    }
    
}