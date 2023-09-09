using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Domain;

public class User : IEntity {

    // Common Entity Properties
    public UserId Id { get; private set; } = UserId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private User() { }
}