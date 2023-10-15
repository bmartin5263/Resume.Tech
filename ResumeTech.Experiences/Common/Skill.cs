using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Common;

public class Skill : IEntity<SkillId> {
    public string Name { get; set; }

    // Common Entity Properties
    public SkillId Id { get; private set; } = SkillId.Generate();

    // Default Constructor Needed for Persistence
    private Skill() {
        Name = null!;
    }

    public Skill(string name) {
        Name = name;
    }
}