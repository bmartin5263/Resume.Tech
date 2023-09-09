using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;
using ResumeTech.Domain.Util;

namespace ResumeTech.Domain.Experience;

public class Project : IEntity {
    
    public string Name { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public Uri? Website { get; set; }
    
    public DateRange Dates { get; set; }
    
    public IList<BulletPoint> BulletPoints { get; set; } = new List<BulletPoint>();
    
    private IList<Image>? _images;
    public IReadOnlyList<Image> Images => _images.ToReadOnly();

    // Common Entity Properties
    public ProjectId Id { get; private set; } = ProjectId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Project() {
        Name = null!;
    }

    public Project(string Name) {
        this.Name = Name;
    }

    public void AddImage(Image image) {
        _images ??= new List<Image>();
        _images.Add(image);
    }
}