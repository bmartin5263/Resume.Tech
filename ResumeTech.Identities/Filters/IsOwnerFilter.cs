using System.Security.Authentication;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;

namespace ResumeTech.Identities.Filters; 

public class IsOwnerFilter<TEntity> : IAccessFilter<TEntity> where TEntity : class, IOwnedEntity {
    
    public Task<bool> CheckCanRead(UserId userId, TEntity entity) {
        return Task.FromResult(entity.OwnerId == userId);
    }
    
}