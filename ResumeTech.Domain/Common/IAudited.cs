namespace ResumeTech.Domain.Common;

public interface IAudited {
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
}

public static class AuditedUtils {
    public static bool IsDeleted<T>(this T obj) where T : IAudited {
        return obj.IsDeleted;
    }
}