using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;
using ResumeTech.Media.Domain;

namespace ResumeTech.Experiences.Projects;

public class Software : IProject {
    public override UserId OwnerId { get; protected set; }

    public ISet<ImageRefId> Images { get; private set; } = new HashSet<ImageRefId>();
    
    // Common Project Properties
    public sealed override string Name { get; set; }
    public sealed override DateOnlyRange Dates { get; set; }
    public override IProjectId ProjectId => Id;

    // Common Entity Properties
    public SoftwareId Id { get; private set; } = SoftwareId.Generate();
    public override DateTimeOffset CreatedAt { get; set; }
    public override DateTimeOffset? UpdatedAt { get; set; }
    public override DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Software() {
        Name = null!;
        Dates = null!;
    }

    public Software(string name) {
        Name = name;
        Dates = new DateOnlyRange(Start: null);
    }
}