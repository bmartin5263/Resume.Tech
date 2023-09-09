using ResumeTech.Common.Domain;

namespace ResumeTech.Common.Repository; 

public interface IRepository<in ID, TEntity> where ID : IEntityId where TEntity : IEntity {
    public TEntity? FindById(ID id);
    public void Add(TEntity entity);
}