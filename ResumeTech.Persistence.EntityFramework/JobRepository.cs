using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Persistence.EntityFramework;

public class JobRepository : GenericSecureRepository<JobId, Job>, IJobRepository {
    public JobRepository(EFCoreContext context, IUserProvider userProvider) : base(context, userProvider) { }

    protected override IQueryable<Job> Entities => base.Entities
        .Include(e => e.Positions);

    protected override void AuthorizeCanRead(Job entity) {
        if (CurrentUser.Id != entity.OwnerId) {
            throw new AuthorizationException();
        }
    }

}