using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.ExperienceManagement.Domain;

public class Job : IEntity {

    // Common Project Properties
    public string CompanyName { get; set; }
    public IProjectId ProjectId => Id;

    // Common Entity Properties
    public JobId Id { get; private set; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Job() {
        CompanyName = null!;
    }

    public Job(string CompanyName) {
        this.CompanyName = CompanyName;
    }
}