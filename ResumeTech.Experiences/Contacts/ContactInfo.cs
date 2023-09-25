using ResumeTech.Common.Attributes;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Contacts;

public class ContactInfo : IEntity<ContactInfoId>, IAuditedEntity, ISoftDeletable {
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

    public ContactInfo(PersonName Name, Location? Address = null) {
        this.Name = Name;
        this.Location = Address.OrElse(new Location());
    }
}