using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Repository;
using ResumeTech.Common.Utility;

namespace ResumeTech.Persistence.EntityFramework; 

public abstract class GenericSecureRepository<ID, TEntity> : IRepository<ID, TEntity> where ID : IEntityId where TEntity : class, IEntity<ID> {
    protected EFCoreContext Context { get; }
    protected virtual IQueryable<TEntity> Entities => Context.Set<TEntity>();
    
    protected IUserDetailsProvider UserDetailsProvider { get; }
    protected UserDetails CurrentUser => UserDetailsProvider.CurrentUser;

    protected GenericSecureRepository(EFCoreContext context, IUserDetailsProvider userDetailsProvider) {
        Context = context;
        UserDetailsProvider = userDetailsProvider;
    }

    private void DoAuthorizeCanRead(TEntity entity) {
        if (!CurrentUser.IsAdmin()) {
            AuthorizeCanRead(entity);
        }
    }
    
    protected abstract void AuthorizeCanRead(TEntity entity);

    public virtual async Task<TEntity?> FindById(ID id) {
        var entity = await Entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (entity is null or ISoftDeletable { IsDeleted: true }) {
            return null;
        }
        DoAuthorizeCanRead(entity);
        return entity;
    }

    public virtual async Task<TEntity?> FindDeletedById(ID id) {
        var entity = await Entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (entity is null or ISoftDeletable { IsDeleted: true }) {
            return null;
        }
        DoAuthorizeCanRead(entity);
        return null;
    }

    public virtual async Task<TEntity> FindByIdOrThrow(ID id) {
        var entity = await FindById(id);
        entity = entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
        DoAuthorizeCanRead(entity);
        return entity;
    }

    public virtual async Task<TEntity> FindByIdOrThrow(ID id, Func<Exception> exceptionFunc) {
        var entity = await FindById(id);
        entity = entity.OrElseThrow(exceptionFunc);
        DoAuthorizeCanRead(entity);
        return entity;
    }

    public virtual async Task<TEntity> FindDeletedByIdOrThrow(ID id) {
        var entity = await FindDeletedById(id);
        entity = entity.OrElseThrow(UserMessage: $"{typeof(TEntity).Name} not found by id: {id}");
        DoAuthorizeCanRead(entity);
        return entity;
    }

    public virtual async Task<TEntity> FindDeletedByIdOrThrow(ID id, Func<Exception> exceptionFunc) {
        var entity = await FindDeletedById(id);
        entity = entity.OrElseThrow(exceptionFunc);
        DoAuthorizeCanRead(entity);
        return entity;
    }

    public virtual void Add(TEntity entity) {
        Context.Set<TEntity>().Add(entity);
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
        Context.Set<TEntity>().Remove(entity);
    }
}