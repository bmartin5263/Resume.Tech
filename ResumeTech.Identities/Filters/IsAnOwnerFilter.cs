using System.Security.Authentication;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;

namespace ResumeTech.Identities.Filters; 

public class IsAnOwnerFilter<TEntity> : IAccessFilter<TEntity> where TEntity : class, IMultiOwnedEntity {
    
    public Task<bool> CheckCanRead(UserId userId, TEntity entity) {
        return Task.FromResult(entity.OwnerIds.Contains(userId));
    }
    
}