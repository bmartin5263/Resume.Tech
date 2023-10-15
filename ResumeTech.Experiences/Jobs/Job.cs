using System.ComponentModel.DataAnnotations;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Jobs;

public sealed class Job : IEntity<JobId>, IAuditedEntity, ISoftDeletable, IEquatable<Job> {
    
    public UserId OwnerId { get; private set; }

    private string companyName = null!;
    public string CompanyName {
        get => companyName;
        set => companyName = value.Validate("Company Name");
    }
    
    public Location Location { get; set; }
    
    public IList<Position> Positions { get; }
    
    public IList<Skill> Skills { get; }
    
    public IList<Tag> Tags { get; }

    // Common Entity Properties
    public JobId Id { get; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = Clock.Now;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Job() {
        CompanyName = null!;
        Location = null!;
        Positions = null!;
        Skills = null!;
        Tags = null!;
    }

    public Job(
        UserId OwnerId, 
        string CompanyName, 
        IEnumerable<Position> Positions,
        IEnumerable<Skill>? Skills = null,
        IEnumerable<Tag>? Tags = null,
        Location? Location = null
    ) {
        this.OwnerId = OwnerId;
        this.CompanyName = CompanyName;
        this.Positions = new List<Position>(Positions).AssertNotEmpty("Positions");
        this.Skills = new List<Skill>(Skills ?? Enumerable.Empty<Skill>());
        this.Tags = new List<Tag>(Tags ?? Enumerable.Empty<Tag>());
        this.Location = Location ?? new Location();
    }
    
    public static bool operator ==(Job? obj1, Job? obj2) => obj1?.Equals(obj2) ?? ReferenceEquals(obj2, null);
    public static bool operator !=(Job? obj1, Job? obj2) => !(obj1 == obj2);

    public bool Equals(Job? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Job)obj);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}