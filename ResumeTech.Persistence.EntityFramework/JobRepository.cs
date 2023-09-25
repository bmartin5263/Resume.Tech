using Microsoft.EntityFrameworkCore;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.EntityFramework;

public class JobRepository : GenericRepository<JobId, Job>, IJobRepository {
    public JobRepository(EFCoreContext context) : base(context) { }

    protected override IQueryable<Job> Entities => base.Entities.Include(e => e.Positions);
}