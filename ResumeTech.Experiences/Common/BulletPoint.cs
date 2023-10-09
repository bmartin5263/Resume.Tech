using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Common;

public class BulletPoint : IEntity<BulletPointId> {
    public string Content { get; set; }

    // Common Entity Properties
    public BulletPointId Id { get; private set; } = BulletPointId.Generate();
    
    // Default Constructor Needed for Persistence
    private BulletPoint() {
        Content = null!;
    }

    public BulletPoint(string content) {
        Content = content;
    }
}