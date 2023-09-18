using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth.Filters; 

public interface IAccessFilter<in TEntity> where TEntity : class {
    Task<bool> CheckCanRead(UserId userId, TEntity entity);
}