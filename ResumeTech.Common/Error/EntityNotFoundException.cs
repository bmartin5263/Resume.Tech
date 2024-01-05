namespace ResumeTech.Common.Error; 

public class EntityNotFoundException : Exception {
    public EntityNotFoundException(string? message) : base(message) { }
    public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }

    public static EntityNotFoundException IdNotFound<TEntity>(object id) {
        return new EntityNotFoundException($"{typeof(TEntity).Name} not found by id {id}");
    }
}