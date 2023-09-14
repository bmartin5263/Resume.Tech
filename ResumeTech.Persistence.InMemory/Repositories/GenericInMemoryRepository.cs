using System.Collections.Concurrent;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;

namespace ResumeTech.Persistence.InMemory.Repositories; 

public abstract class GenericInMemoryRepository<ID, TEntity> : IRepository<ID, TEntity> 
    where TEntity : class, IEntity<ID> 
    where ID : IEntityId {
    
    protected IDictionary<ID, TEntity> Datastore { get; } = new ConcurrentDictionary<ID, TEntity>();

    public TEntity? FindById(ID id) {
        return Datastore.Find(id);
    }

    public void Add(TEntity entity) {
        Datastore[entity.Id] = entity;
    }
    
}