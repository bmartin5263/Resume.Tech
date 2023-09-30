using ResumeTech.Common.Attributes;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Contacts;

public class ContactInfo : IEntity<ContactInfoId>, IAuditedEntity, ISoftDeletable, IOwnedEntity {
    public UserId OwnerId { get; private set; }
    
    [Pii] public PersonName Name { get; set; }
    [Pii] public Location Location { get; set; }
    [Pii] public IList<Link> Links { get; private set; } = new List<Link>();

    // Common Entity Properties
    public ContactInfoId Id { get; private set; } = ContactInfoId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ContactInfo() {
        Name = null!;
        Location = null!;
    }

    public ContactInfo(UserId OwnerId, PersonName Name, Location? Location = null) {
        this.OwnerId = OwnerId;
        this.Name = Name;
        this.Location = Location.OrElse(new Location());
    }
}