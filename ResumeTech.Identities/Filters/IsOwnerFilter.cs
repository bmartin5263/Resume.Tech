using System.Security.Authentication;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Exceptions;

namespace ResumeTech.Identities.Filters; 

public class IsOwnerFilter<TEntity> : IAccessFilter<TEntity> where TEntity : IOwnedEntity {
    
    public bool CheckCanRead(UserId userId, TEntity entity) {
        return entity.OwnerId == userId;
    }

    public bool CheckCanWrite(UserId userId, TEntity entity) {
        return CheckCanRead(userId, entity);
    }
}