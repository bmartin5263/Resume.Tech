using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.InMemory.Repositories; 

public class JobInMemoryRepository : GenericInMemoryRepository<JobId, Job>, IJobRepository {
    
}