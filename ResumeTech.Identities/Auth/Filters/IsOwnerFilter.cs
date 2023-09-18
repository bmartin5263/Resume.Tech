using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth.Filters; 

public class IsOwnerFilter<TEntity> : IAccessFilter<TEntity> 
    where TEntity : class, IOwnedEntity 
{
    public Task<bool> CheckCanRead(UserId userId, TEntity entity) {
        return Task.FromResult(entity.OwnerId == userId);
    }
}