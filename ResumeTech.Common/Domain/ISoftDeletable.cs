namespace ResumeTech.Common.Domain;

public interface ISoftDeletable {
    public DateTimeOffset? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
}