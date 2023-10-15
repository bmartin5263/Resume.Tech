using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.EntityFramework;

public class JobRepository : GenericSecureRepository<JobId, Job>, IJobRepository {
    public JobRepository(EFCoreContext context, IUserDetailsProvider userDetailsProvider) : base(context, userDetailsProvider) { }

    protected override IQueryable<Job> Entities => base.Entities
        .Include(e => e.Positions);

    protected override void AuthorizeCanRead(Job entity) {
        if (CurrentUser.Id != entity.OwnerId) {
            throw new AccessDeniedException();
        }
    }

}