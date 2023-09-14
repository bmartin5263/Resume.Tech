using ResumeTech.Identities.Domain;

namespace ResumeTech.Identities.Filters; 

public interface IAccessFilter<in TEntity> {
    bool CheckCanRead(UserId userId, TEntity entity);
    bool CheckCanWrite(UserId userId, TEntity entity);
}