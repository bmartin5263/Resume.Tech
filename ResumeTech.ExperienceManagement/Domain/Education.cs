using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.ExperienceManagement.Domain;

public class Education : IEntity {
    
    public string Name { get; set; }
    
    public Address? Address { get; set; }
    
    public DegreeType DegreeType { get; set; }

    public string AreaOfStudy { get; set; }
    
    public Gpa? Gpa { get; set; }
    
    public IList<BulletPoint> BulletPoints { get; set; } = new List<BulletPoint>();

    // Common Entity Properties
    public EducationId Id { get; private set; } = EducationId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Education() {
        Name = null!;
        AreaOfStudy = null!;
    }

    public Education(string name, DegreeType degreeType, string areaOfStudy) {
        Name = name;
        DegreeType = degreeType;
        AreaOfStudy = areaOfStudy;
    }
}