using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Jobs;

public sealed class Position : IEntity<PositionId>, IAuditedEntity, IEquatable<Position> {

    /// <summary>
    /// The name of the title held while in this position
    /// </summary>
    private string title = null!;
    public string Title {
        get => title;
        set => title = value.AssertValid("Title");
    }
    
    /// <summary>
    /// The dates this position was held
    /// </summary>
    public DateOnlyRange Dates { get; set; }
    
    /// <summary>
    /// Long form description of what was done while in this position
    /// </summary>
    public const int MaxDescriptionLen = 500;
    private string? description;
    public string? Description {
        get => description;
        set => description = value.AssertNullableValid("Description", max: MaxDescriptionLen);
    }
    
    /// <summary>
    /// Short form bullet points describing duties and accomplishments
    /// </summary>
    public IList<BulletPoint> BulletPoints { get; }
    
    // Common Entity Properties
    public PositionId Id { get; } = PositionId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = Clock.Now;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Position() {
        Title = null!;
        Dates = null!;
        BulletPoints = null!;
    }

    public Position(
        string Title, 
        DateOnlyRange Dates, 
        IEnumerable<BulletPoint>? BulletPoints = null,
        string? Description = null
    ) {
        this.Title = Title;
        this.Dates = Dates;
        this.Description = Description;
        this.BulletPoints = new List<BulletPoint>(BulletPoints ?? Enumerable.Empty<BulletPoint>());
    }
    
    public static bool operator ==(Position? obj1, Position? obj2) => obj1?.Equals(obj2) ?? ReferenceEquals(obj2, null);
    public static bool operator !=(Position? obj1, Position? obj2) => !(obj1 == obj2);

    public bool Equals(Position? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Position)obj);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}