using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Education;
using ResumeTech.Experiences.Education.Degrees;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Organization.Resumes;

public class ResumeDegree : IEntity<ResumeEducationId>, IAuditedEntity {
    public DegreeId DegreeId { get; set; }
    public IList<ResumeBulletPoint> BulletPoints { get; set; }

    // Common Entity Properties
    public ResumeEducationId Id { get; private set; } = ResumeEducationId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumeDegree() {
        BulletPoints = new List<ResumeBulletPoint>();
    }

    public ResumeDegree(DegreeId DegreeId, IEnumerable<ResumeBulletPoint> BulletPoints) {
        this.DegreeId = DegreeId;
        this.BulletPoints = new List<ResumeBulletPoint>(BulletPoints);
    }
}