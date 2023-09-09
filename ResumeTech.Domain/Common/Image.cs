using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.Domain.Common; 

public readonly record struct ImageId(Guid Value) : IEntityId {
    public static ImageId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}

public class Image : IEntity {
    public Uri Location { get; set; }

    // Common Entity Properties
    public ImageId Id { get; private set; } = ImageId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Image() {
        Location = null!;
    }

    public Image(Uri Location) {
        this.Location = Location;
    }
}