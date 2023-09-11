using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Experiences.Jobs; 

public class JobManager {
    private IJobRepository JobRepository { get; }
    
    public JobManager(IJobRepository jobRepository) {
        JobRepository = jobRepository;
    }

    public JobDto CreateJob(CreateJobRequest request) {
        var job = new Job(Owner: UserId.Generate(), CompanyName: request.Name);
        JobRepository.Add(job);
        return job.ToDto();
    }
    
    public JobDto GetJobById(GetJobByIdRequest request) {
        return JobRepository.FindById(request.Id)
            .OrElseThrow(() => new ArgumentException($"Job not found by id: {request.Id}"))
            .ToDto();
    }
    
    public JobDto UpdateJob(PatchJobRequest request) {
        var job = JobRepository.FindById(request.Id)
            .OrElseThrow(() => new ArgumentException($"Job not found by id: {request.Id}"));
        request.ApplyTo(job);
        return job.ToDto();
    }
}