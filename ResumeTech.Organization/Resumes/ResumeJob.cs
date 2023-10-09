using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Organization.Resumes;

public class ResumeJob : IEntity<ResumeJobId>, IAuditedEntity {
    public JobId JobId { get; set; }
    public ISet<BulletPointId> ExcludeBulletPoints { get; set; } = new HashSet<BulletPointId>();

    // Common Entity Properties
    public ResumeJobId Id { get; private set; } = ResumeJobId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumeJob() {
        
    }

    public ResumeJob(JobId JobId, IEnumerable<BulletPointId>? ExcludeBulletPoints = null) {
        this.JobId = JobId;
        this.ExcludeBulletPoints = new HashSet<BulletPointId>(ExcludeBulletPoints ?? Array.Empty<BulletPointId>());
    }
}