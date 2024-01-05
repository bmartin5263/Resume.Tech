using ResumeTech.Common.Domain;
using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Organization.Resumes;

public class ResumeBulletPoint : IEntity<ResumeBulletPointId>, IAuditedEntity, ISoftDeletable {
    public BulletPointId? ReferenceBulletPointId { get; set; }
    public string? Content { get; set; }

    // Common Entity Properties
    public ResumeBulletPointId Id { get; private set; } = ResumeBulletPointId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumeBulletPoint() { }

    public ResumeBulletPoint(BulletPointId? ReferenceBulletPointId, string? Content = null) {
        this.ReferenceBulletPointId = ReferenceBulletPointId;
        this.Content = Content;

        if (ReferenceBulletPointId == null && Content.IsBlank()) {
            throw new ArgumentException("Resume Bullet Points must have content if they do not reference another");
        }
    }
}