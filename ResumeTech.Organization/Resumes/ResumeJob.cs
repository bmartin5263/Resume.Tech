using ResumeTech.Common.Domain;
using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Organization.Resumes;

public class ResumeJob : IEntity<ResumeJobId>, IAuditedEntity {
    public JobId JobId { get; set; }
    public IList<ResumePosition> ResumePositions { get; set; }

    // Common Entity Properties
    public ResumeJobId Id { get; private set; } = ResumeJobId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ResumeJob() {
        ResumePositions = new List<ResumePosition>();
    }

    public ResumeJob(JobId JobId, IEnumerable<ResumePosition> Positions) {
        this.JobId = JobId;
        this.ResumePositions = new List<ResumePosition>(Positions);

        if (this.ResumePositions.IsEmpty()) {
            throw new ArgumentException("Must provide at least one Position");
        }
    }
}