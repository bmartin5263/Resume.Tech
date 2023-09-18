using ResumeTech.Cqs;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.EntityFramework; 

public class JobRepository : GenericRepository<JobId, Job>, IJobRepository {
    public JobRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
}