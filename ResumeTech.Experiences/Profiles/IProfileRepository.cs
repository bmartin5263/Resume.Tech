using ResumeTech.Common.Auth;
using ResumeTech.Common.Repository;

namespace ResumeTech.Experiences.Profiles;

public interface IProfileRepository : IRepository<ProfileId, Profile> {
    Task<Profile?> FindByUserId(UserId userId);
}