using ResumeTech.Experiences.Contacts;

namespace ResumeTech.Experiences.Jobs.Dto; 

public sealed record CreateJobRequest(
    string CompanyName,
    Location? Location,
    IList<AddPositionRequest> Positions
);
