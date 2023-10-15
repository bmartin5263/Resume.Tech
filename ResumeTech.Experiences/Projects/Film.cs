using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Projects;

public class Film : IProject {
    public override UserId OwnerId { get; protected set; }

    public string Title { get; set; }
    
    // Common Project Properties
    public sealed override string Name { get; set; }
    public sealed override DateOnlyRange Dates { get; set; }
    public override IProjectId ProjectId => Id;

    // Common Entity Properties
    public FilmId Id { get; private set; } = FilmId.Generate();
    public override DateTimeOffset CreatedAt { get; set; }
    public override DateTimeOffset? UpdatedAt { get; set; }
    public override DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Film() {
        Name = null!;
        Dates = null!;
        Title = null!;
    }

    public Film(string name, string title) {
        Name = name;
        Title = title;
        Dates = new DateOnlyRange();
    }
}