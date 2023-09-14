using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Experiences.Jobs; 

public sealed record GetJobByIdRequest(JobId Id);