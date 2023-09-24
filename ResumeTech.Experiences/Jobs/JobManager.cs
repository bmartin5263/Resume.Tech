using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Jobs; 

public class JobManager {
    private SecureRepository<JobId, Job, IJobRepository> JobRepository { get; }

    public JobManager(IJobRepository jobRepository, Authorizer<Job> authorizer) {
        JobRepository = new SecureRepository<JobId, Job, IJobRepository>(jobRepository, authorizer);
    }

    public JobDto CreateJob(CreateJobRequest request) {
        var userId = JobRepository.CurrentUserId;
        var job = new Job(OwnerId: userId, CompanyName: request.Name);
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