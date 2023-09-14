using System.Security.Authentication;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;

namespace ResumeTech.Identities.Filters; 

public class IsAnOwnerFilter<TEntity> : IAccessFilter<TEntity> where TEntity : IMultiOwnedEntity {
    
    public bool CheckCanRead(UserId userId, TEntity entity) {
        return entity.OwnerIds.Contains(userId);
    }

    public bool CheckCanWrite(UserId userId, TEntity entity) {
        return CheckCanRead(userId, entity);
    }
}