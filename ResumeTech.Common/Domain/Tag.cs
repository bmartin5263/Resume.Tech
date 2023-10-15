namespace ResumeTech.Common.Domain;

public class Tag : IEntity<TagId> {
    public string Name { get; set; }

    // Common Entity Properties
    public TagId Id { get; private set; } = TagId.Generate();

    // Default Constructor Needed for Persistence
    private Tag() {
        Name = null!;
    }

    public Tag(string name) {
        Name = name;
    }
}