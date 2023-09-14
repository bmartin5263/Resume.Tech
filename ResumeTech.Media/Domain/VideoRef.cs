using ResumeTech.Common.Domain;

namespace ResumeTech.Media.Domain;

public class VideoRef : IEntity<VideoRefId>, IAuditedEntity, ISoftDeletable {
    public Uri Location { get; private set; }

    // Common Entity Properties
    public VideoRefId Id { get; private set; } = VideoRefId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private VideoRef() {
        Location = null!;
    }

    public VideoRef(Uri Location) {
        this.Location = Location;
    }
}