using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs.Dto;
using ResumeTech.Experiences.Profiles;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Jobs; 

public class JobManager {
    private SecureRepository<ProfileId, Profile, IProfileRepository> ProfileRepository { get; }
    private SecureRepository<JobId, Job, IJobRepository> JobRepository { get; }

    public JobManager(
        IProfileRepository profileRepository, 
        IJobRepository jobRepository, 
        Authorizer<Job> jobAuthorizer,
        Authorizer<Profile> profileAuthorizer
    ) {
        ProfileRepository =
            new SecureRepository<ProfileId, Profile, IProfileRepository>(profileRepository, profileAuthorizer);
        JobRepository = new SecureRepository<JobId, Job, IJobRepository>(jobRepository, jobAuthorizer);
    }

    public async Task<JobDto> CreateJob(CreateJobRequest request) {
        var userId = JobRepository.CurrentUserId;
        var profile = (await ProfileRepository.ReadNullable(r => r.FindByUserId(userId))).OrElseThrow();
        
        var job = new Job(
            OwnerId: profile.Id,
            Location: request.Location,
            CompanyName: request.CompanyName,
            Positions: request.Positions.Select(p => p.ToEntity())
        );
        
        JobRepository.Add(job);
        return job.ToDto();
    }

    public async Task<JobDto?> GetJobById(GetJobByIdRequest request) {
        var job = await JobRepository.ReadNullable(r => r.FindById(request.Id));
        return job?.ToDto();
    }
    
    public async Task<JobDto> UpdateJob(PatchJobRequest request) {
        var job = await JobRepository.Read(r => r.FindByIdOrThrow(request.Id));
        request.ApplyTo(job);
        return job.ToDto();
    }
    
    public async Task DeleteJob(DeleteJobRequest request) {
        var job = await JobRepository.ReadNullable(r => r.FindById(request.Id));
        if (job != null) {
            JobRepository.Delete(job);
        }
    }
    
    public async Task PurgeJob(DeleteJobRequest request) {
        var job = await JobRepository.ReadNullable(r => r.FindById(request.Id));
        if (job != null) {
            JobRepository.Purge(job);
        }
    }
}