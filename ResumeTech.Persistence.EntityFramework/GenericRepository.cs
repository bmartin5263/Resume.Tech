using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;

namespace ResumeTech.Persistence.EntityFramework; 

public abstract class GenericRepository<ID, TEntity> : IRepository<ID, TEntity> where ID : IEntityId where TEntity : class, IEntity<ID> {
    protected EFCoreContext Context { get; }
    protected DbSet<TEntity> Entities => Context.Set<TEntity>();

    public GenericRepository(EFCoreContext context) {
        Context = context;
    }

    public virtual async Task<TEntity?> FindById(ID id) {
        var entity = await Entities.FindAsync(id);
        if (entity is ISoftDeletable { IsDeleted: true }) {
            return null;
        }
        return entity;
    }

    public virtual async Task<TEntity?> FindDeletedById(ID id) {
        var entity = await Entities.FindAsync(id);
        if (entity is ISoftDeletable { IsDeleted: true }) {
            return entity;
        }
        return null;
    }

    public virtual async Task<TEntity> FindByIdOrThrow(ID id) {
        var entity = await FindById(id);
        return entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
    }

    public virtual async Task<TEntity> FindByIdOrThrow(ID id, Func<Exception> exceptionFunc) {
        var entity = await FindById(id);
        return entity.OrElseThrow(exceptionFunc);
    }

    public virtual async Task<TEntity> FindDeletedByIdOrThrow(ID id) {
        var entity = await FindDeletedById(id);
        return entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
    }

    public virtual async Task<TEntity> FindDeletedByIdOrThrow(ID id, Func<Exception> exceptionFunc) {
        var entity = await FindDeletedById(id);
        return entity.OrElseThrow(exceptionFunc);
    }

    public virtual void Add(TEntity entity) {
        Entities.Add(entity);
    }

    public virtual void Delete(TEntity entity) {
        if (entity is ISoftDeletable softDeletable) {
            softDeletable.DeletedAt = DateTimeOffset.UtcNow;
        }
        else {
            Purge(entity);
        }
    }

    public virtual void Purge(TEntity entity) {
        Entities.Remove(entity);
    }
}