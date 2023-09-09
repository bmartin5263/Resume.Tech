using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Jobs; 

public sealed record CreateJobRequest(
    string Name,
    DateOnlyRange Dates
);