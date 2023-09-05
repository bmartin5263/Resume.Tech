namespace ResumeTech.Domain.Experience; 

public class ProjectExperience {
    public string ProjectName { get; set; }
    public string? Description { get; set; }
    public string? Company { get; set; }
    public Uri? Website { get; set; }
    public IList<JobPosition> Positions { get; set; } = new List<JobPosition>();

    public ProjectExperience(string projectName) {
        ProjectName = projectName;
    }
}