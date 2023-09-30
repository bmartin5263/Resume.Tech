using System.ComponentModel.DataAnnotations;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Profiles;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Jobs;

public class Job : IEntity<JobId>, IAuditedEntity, ISoftDeletable, IOwnedEntity<ProfileId> {
    public const int MaxFieldLength = 80;

    public ProfileId OwnerId { get; private set; }
    public string CompanyName { get; set; }
    public Location Location { get; set; }
    public IList<Position> Positions { get; } = new List<Position>();

    // Common Entity Properties
    public JobId Id { get; private set; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Job() {
        CompanyName = null!;
        Location = null!;
    }

    public Job(ProfileId OwnerId, string CompanyName, IEnumerable<Position> Positions, Location? Location = null) {
        this.OwnerId = OwnerId;
        this.CompanyName = CompanyName.AssertMaxTrimmedLength(MaxFieldLength, "companyName");
        this.Location = Location ?? new Location();
        this.Positions = new List<Position>(Positions).AssertNotEmpty("positions");
    }
}