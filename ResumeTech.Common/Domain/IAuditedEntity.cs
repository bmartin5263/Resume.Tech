namespace ResumeTech.Common.Domain;

public interface IAuditedEntity {
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}