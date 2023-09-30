using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Auth.Filters; 

public interface IAccessFilter<in TEntity> where TEntity : class {
    Task<bool> CheckCanRead(UserDetails user, TEntity entity);
}