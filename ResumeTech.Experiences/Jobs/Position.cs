using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Jobs;

public class Position : IEntity<PositionId>, IAuditedEntity {
    
    /// <summary>
    /// The name of the title held while in this position
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// The dates this position was held
    /// </summary>
    public DateOnlyRange Dates { get; set; }
    
    /// <summary>
    /// Long form description of what was done while in this position
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Short form bullet points describing duties and accomplishments
    /// </summary>
    public IList<BulletPoint> BulletPoints { get; } = new List<BulletPoint>();

    // Common Entity Properties
    public PositionId Id { get; private set; } = PositionId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Position() {
        Title = null!;
        Dates = null!;
    }

    public Position(string Title, DateOnlyRange Dates, IEnumerable<BulletPoint>? BulletPoints = null) {
        this.Title = Title;
        this.Dates = Dates;
        this.BulletPoints = BulletPoints?.ToList() ?? new List<BulletPoint>();
    }
}