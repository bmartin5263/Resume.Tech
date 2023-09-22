using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Educations;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Projects;

namespace ResumeTech.Experiences.Profiles;

public class Profile : IEntity<ProfileId>, IAuditedEntity, ISoftDeletable {
    public UserId OwnerId { get; private set; }
    
    // Read-only references
    private IList<ContactInfo>? ContactInfos { get; set; }
    private IList<Job>? Jobs { get; set; }
    private IList<IProject>? Projects { get; set; }
    private IList<Education>? Educations { get; set; }

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