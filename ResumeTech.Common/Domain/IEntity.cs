namespace ResumeTech.Common.Domain;

public interface IEntity {
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
}

public static class AuditedUtils {
    public static bool IsDeleted<T>(this T obj) where T : IEntity {
        return obj.IsDeleted;
    }
}