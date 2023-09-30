using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Educations;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Projects;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Profiles;

public class Profile : IEntity<ProfileId>, IAuditedEntity, ISoftDeletable, IOwnedEntity<UserId> {
    public UserId OwnerId { get; private set; }
    
    // Read-only references
    public IList<ContactInfo>? ContactInfos { get; set; }
    public IList<Job>? Jobs { get; set; }
    // public IList<IProject>? Projects { get; set; }
    public IList<Education>? Educations { get; set; }

    // Common Entity Properties
    public ProfileId Id { get; private set; } = ProfileId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Profile() { }

    public Profile(UserId ownerId) {
        OwnerId = ownerId;
    }
}