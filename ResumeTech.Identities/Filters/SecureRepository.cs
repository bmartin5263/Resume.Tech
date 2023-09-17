using ResumeTech.Common.Domain;
using ResumeTech.Common.Repository;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Util;

namespace ResumeTech.Identities.Filters; 

public class SecureRepository<ID, TEntity, TRepository> 
    where ID : IEntityId 
    where TEntity : class, IEntity<ID> 
    where TRepository : IRepository<ID, TEntity> 
{
    private TRepository Impl { get; }
    private Authorizer<TEntity> Authorizer { get; }
    public UserId CurrentUser => Authorizer.CurrentUser;
    
    public SecureRepository(TRepository Impl, Authorizer<TEntity> Authorizer) {
        this.Impl = Impl;
        this.Authorizer = Authorizer;
    }

    public async Task<TEntity> Read(Func<TRepository, Task<TEntity>> operation) {
        var result = await operation(Impl);
        return await Authorizer.AssertCanRead(result);
    }
    
    public async Task<TEntity?> ReadNullable(Func<TRepository, Task<TEntity?>> operation) {
        var result = await operation(Impl);
        return result == null ? null : await Authorizer.AssertCanRead(result);
    }

    public void Add(TEntity entity) {
        Impl.Add(entity);
    }
    
    public void Delete(TEntity entity) {
        Impl.Delete(entity);
    }
}
