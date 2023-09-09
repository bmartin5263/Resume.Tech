using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Resources;
using ResumeTech.Resources.Domain;

namespace ResumeTech.ExperienceManagement.Domain;

public class Software : IProject, IEntity {
    public ISet<ImageRefId> Images { get; private set; } = new HashSet<ImageRefId>();
    
    // Common Project Properties
    public string Name { get; set; }
    public DateOnlyRange Dates { get; set; }
    public IProjectId ProjectId => Id;

    // Common Entity Properties
    public SoftwareId Id { get; private set; } = SoftwareId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Software() {
        Name = null!;
    }

    public Software(string name) {
        Name = name;
    }
}