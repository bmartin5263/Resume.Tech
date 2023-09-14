using ResumeTech.Common.Repository;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Filters;

namespace ResumeTech.Experiences.Jobs;

public interface IJobRepository : IRepository<JobId, Job> {
}