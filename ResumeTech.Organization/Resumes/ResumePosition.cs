using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Organization.Resumes;

public class ResumePosition : IEntity<ResumePositionId>, IAuditedEntity, ISoftDeletable {
    public PositionId PositionId { get; set; }
    public IList<ResumeBulletPoint> BulletPoints { get; set; } = new List<ResumeBulletPoint>();

    // Common Entity Properties
    public ResumePositionId Id { get; private set; } = ResumePositionId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumePosition() { }

    public ResumePosition(PositionId PositionId) {
        this.PositionId = PositionId;
    }
}