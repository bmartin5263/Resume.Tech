using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;

namespace ResumeTech.Experiences.Education.Degrees;

public class Degree : IEntity<DegreeId>, IAuditedEntity, ISoftDeletable {
    public UserId OwnerId { get; private set; }

    private string institutionName = null!;
    public string InstitutionName {
        get => institutionName;
        set => institutionName = value.AssertValid("institutionName");
    }
    
    public Location Location { get; set; } = new();
    
    public DegreeType DegreeType { get; set; }
    
    private string? major;
    public string? Major {
        get => major;
        set => major = value.AssertNullableValid("major");
    }
    
    private string? minor;
    public string? Minor {
        get => minor;
        set => minor = value.AssertNullableValid("minor");
    }
    
    public Gpa? Gpa { get; set; }
    
    public IList<BulletPoint> BulletPoints { get; set; } = new List<BulletPoint>();

    // Common Entity Properties
    public DegreeId Id { get; private set; } = DegreeId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Degree() {
        InstitutionName = null!;
    }
    
    public Degree(
        string InstitutionName, 
        DegreeType DegreeType, 
        Location? Location = null,
        string? Major = null,
        string? Minor = null,
        Gpa? Gpa = null,
        IEnumerable<BulletPoint>? BulletPoints = null
    ) {
        this.InstitutionName = InstitutionName;
        this.DegreeType = DegreeType;
        this.Location = Location.OrElse(new Location());
        this.Major = Major;
        this.Minor = Minor;
        this.Gpa = Gpa;
        this.BulletPoints = new List<BulletPoint>(BulletPoints ?? Enumerable.Empty<BulletPoint>());

        if (DegreeType != DegreeType.HighSchoolDiploma && Major == null) {
            throw new ArgumentException("Major is required when Degree Type is null");
        }
    }
}