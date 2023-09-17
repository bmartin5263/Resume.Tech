using System.Collections.Concurrent;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;

namespace ResumeTech.Persistence.InMemory.Repositories; 

public abstract class GenericInMemoryRepository<ID, TEntity> : IRepository<ID, TEntity> 
    where TEntity : class, IEntity<ID> 
    where ID : IEntityId {
    
    protected IDictionary<ID, TEntity> Datastore { get; } = new ConcurrentDictionary<ID, TEntity>();

    public Task<TEntity?> FindById(ID id) {
        var entity = Datastore.Find(id);
        if (entity is ISoftDeletable softDeletable) {
            return softDeletable.IsDeleted ? Task.FromResult<TEntity?>(null) : Task.FromResult<TEntity?>(entity);
        }
        return Task.FromResult(entity);
    }

    public Task<TEntity?> FindDeletedById(ID id) {
        var entity = Datastore.Find(id);
        if (entity is ISoftDeletable softDeletable) {
            return softDeletable.IsDeleted ? Task.FromResult<TEntity?>(entity) : Task.FromResult<TEntity?>(null);
        }
        return Task.FromResult<TEntity?>(null);
    }

    public async Task<TEntity> FindByIdOrThrow(ID id) {
        var entity = await FindById(id);
        return entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
    }

    public async Task<TEntity> FindByIdOrThrow(ID id, Func<Exception> elseThrow) {
        var entity = await FindById(id);
        return entity.OrElseThrow(elseThrow);
    }

    public async Task<TEntity> FindDeletedByIdOrThrow(ID id) {
        var entity = await FindDeletedById(id);
        return entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
    }

    public async Task<TEntity> FindDeletedByIdOrThrow(ID id, Func<Exception> elseThrow) {
        var entity = await FindDeletedById(id);
        return entity.OrElseThrow(elseThrow);
    }
    
    public void Add(TEntity entity) {
        Datastore[entity.Id] = entity;
    }

    public void Delete(TEntity entity) {
        if (entity is ISoftDeletable softDeletable) {
            softDeletable.DeletedAt = DateTimeOffset.UtcNow;
        }
        else {
            Datastore.Remove(entity.Id);
        }
    }
    
    public void Purge(TEntity entity) {
        Datastore.Remove(entity.Id);
    }
}