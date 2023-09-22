using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.EntityFramework; 

public class JobRepository : GenericRepository<JobId, Job>, IJobRepository {
    public JobRepository(EFCoreContext context) : base(context) { }
}