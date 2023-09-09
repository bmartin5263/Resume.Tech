using ResumeTech.ExperienceManagement.Domain;

namespace ResumeTech.ExperienceManagement.Dto; 

public sealed record CreateJobRequest(
    string Name,
    DateOnlyRange Dates
);