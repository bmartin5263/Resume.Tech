using ResumeTech.Identities.Domain;

namespace ResumeTech.Identities.Filters; 

public interface IAccessFilter<in TEntity> where TEntity : class {
    Task<bool> CheckCanRead(UserId userId, TEntity entity);
}