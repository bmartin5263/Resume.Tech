using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Identities.Auth.Filters; 

public class IsOwnerFilter<TEntity, TOwnerId> : IAccessFilter<TEntity> 
    where TEntity : class, IOwnedEntity<TOwnerId>
    where TOwnerId : notnull
{
    public Task<bool> CheckCanRead(UserDetails user, TEntity entity) {
        return Task.FromResult(user.Id.Equals(entity.OwnerId));
    }
}