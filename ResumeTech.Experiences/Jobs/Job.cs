using ResumeTech.Common.Domain;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Experiences.Jobs;

public class Job : IEntity<JobId>, IAuditedEntity, ISoftDeletable, IOwnedEntity {
    public UserId OwnerId { get; private set; }
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

    public Job(UserId OwnerId, string CompanyName) {
        this.OwnerId = OwnerId;
        this.CompanyName = CompanyName;
    }
}