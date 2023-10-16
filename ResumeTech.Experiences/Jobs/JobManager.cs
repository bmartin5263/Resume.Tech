using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs.Dto;
using ResumeTech.Identities.Auth;

namespace ResumeTech.Experiences.Jobs; 

public class JobManager {
    private IJobRepository JobRepository { get; }
    private IUserProvider UserProvider { get; }

    public JobManager(IJobRepository jobRepository, IUserProvider userProvider) {
        JobRepository = jobRepository;
        UserProvider = userProvider;
    }

    public Task<JobDto> CreateJob(CreateJobRequest request) {
        var job = new Job(
            OwnerId: UserProvider.CurrentUserId,
            Location: request.Location,
            CompanyName: request.CompanyName,
            Positions: request.Positions.Select(p => p.ToEntity())
        );
        
        JobRepository.Add(job);
        return Task.FromResult(job.ToDto());
    }

    public async Task<JobDto?> GetJobById(GetJobByIdRequest request) {
        var job = await JobRepository.FindById(request.Id);
        return job?.ToDto();
    }
    
    public async Task<JobDto> UpdateJob(PatchJobRequest request) {
        var job = await JobRepository.FindByIdOrThrow(request.Id);
        request.ApplyTo(job);
        return job.ToDto();
    }
    
    public async Task DeleteJob(DeleteJobRequest request, bool purge = false) {
        var job = await JobRepository.FindById(request.Id);
        if (job != null) {
            if (purge) {
                JobRepository.Purge(job);
            }
            else {
                JobRepository.Delete(job);
            }
        }
    }
}