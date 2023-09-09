using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.ExperienceManagement.Domain;

public class Job : IProject, IEntity {

    // Common Project Properties
    public string Name { get; set; }
    public DateOnlyRange Dates { get; set; }
    public IProjectId ProjectId => Id;

    // Common Entity Properties
    public JobId Id { get; private set; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Job() {
        Name = null!;
    }

    public Job(string Name) {
        this.Name = Name;
    }
}