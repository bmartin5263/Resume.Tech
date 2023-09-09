using ResumeTech.Common.Domain;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Experiences.Jobs;

public class Job : IEntity {
    public UserId Owner { get; private set; }
    public string CompanyName { get; set; }

    // Common Entity Properties
    public JobId Id { get; private set; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Job() {
        CompanyName = null!;
    }

    public Job(UserId Owner, string CompanyName) {
        this.Owner = Owner;
        this.CompanyName = CompanyName;
    }
}