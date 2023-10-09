using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Educations;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Organization.Resumes;

public class ResumeEducation : IEntity<ResumeEducationId>, IAuditedEntity {
    public EducationId EducationId { get; set; }

    // Common Entity Properties
    public ResumeEducationId Id { get; private set; } = ResumeEducationId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumeEducation() {
        
    }

    public ResumeEducation(EducationId EducationId, IEnumerable<BulletPointId>? ExcludeBulletPoints = null) {
        this.EducationId = EducationId;
    }
}