using ResumeTech.Common.Domain;

namespace ResumeTech.Common.Repository; 

public interface IRepository<in ID, TEntity> 
    where ID : IEntityId 
    where TEntity : class, IEntity<ID> 
{
    public Task<TEntity?> FindById(ID id);
    
    public Task<TEntity?> FindDeletedById(ID id);
    
    public Task<TEntity> FindByIdOrThrow(ID id);
    
    public Task<TEntity> FindByIdOrThrow(ID id, Func<Exception> exceptionFunc);
    
    public Task<TEntity> FindDeletedByIdOrThrow(ID id);
    
    public Task<TEntity> FindDeletedByIdOrThrow(ID id, Func<Exception> exceptionFunc);
    
    public void Add(TEntity entity);

    public void Delete(TEntity entity);

    public void Purge(TEntity entity);
}