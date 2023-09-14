using ResumeTech.Common.Domain;

namespace ResumeTech.Common.Repository; 

public interface IRepository<in ID, TEntity> 
    where ID : IEntityId 
    where TEntity : class, IEntity<ID> 
{
    public TEntity? FindById(ID id);
    public void Add(TEntity entity);
}