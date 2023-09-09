using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Projects; 

public interface IProject {
    public IProjectId ProjectId { get; }
    public string Name { get; set; }
    public DateOnlyRange Dates { get; set; }
}