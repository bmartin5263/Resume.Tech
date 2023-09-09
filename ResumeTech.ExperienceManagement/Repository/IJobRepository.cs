using ResumeTech.Common.Repository;
using ResumeTech.ExperienceManagement.Domain;

namespace ResumeTech.ExperienceManagement.Repository;

public interface IJobRepository : IRepository<JobId, Job> {
    
}