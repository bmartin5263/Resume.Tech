namespace ResumeTech.Domain.Experience; 

public class Skill {
    public string Name { get; set; }

    private Skill() {
        Name = null!;
    }

    public Skill(string name) {
        Name = name;
    }
}