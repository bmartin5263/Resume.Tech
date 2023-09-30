using Microsoft.EntityFrameworkCore;
using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Profiles;

namespace ResumeTech.Persistence.EntityFramework;

public class ProfileRepository : GenericRepository<ProfileId, Profile>, IProfileRepository {
    public ProfileRepository(EFCoreContext context) : base(context) { }
    
    public Task<Profile?> FindByUserId(UserId userId) {
        throw new NotImplementedException();
    }
}