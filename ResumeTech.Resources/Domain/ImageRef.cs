using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.Resources.Domain;

public class ImageRef : IEntity {
    public Uri Location { get; private set; }

    // Common Entity Properties
    public ImageRefId Id { get; private set; } = ImageRefId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private ImageRef() {
        Location = null!;
    }

    public ImageRef(Uri location) {
        Location = location;
    }
}