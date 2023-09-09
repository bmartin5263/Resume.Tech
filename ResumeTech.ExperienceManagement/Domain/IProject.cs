namespace ResumeTech.ExperienceManagement.Domain; 

public interface IProject {
    public IProjectId ProjectId { get; }
    public string Name { get; set; }
    public DateOnlyRange Dates { get; set; }
}