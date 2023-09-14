using ResumeTech.Common.Utility;
using ResumeTech.Identities.Domain;
using ResumeTech.Identities.Util;

namespace ResumeTech.Experiences.Jobs; 

public class JobManager {
    private IJobRepository JobRepository { get; }
    private Authorizer<Job> JobAuthorizer { get; }

    public JobManager(IJobRepository jobRepository, Authorizer<Job> jobAuthorizer) {
        JobRepository = jobRepository;
        JobAuthorizer = jobAuthorizer;
    }

    public JobDto CreateJob(CreateJobRequest request) {
        var job = new Job(Owner: request.UserId, CompanyName: request.Name);
        JobRepository.Add(job);
        return job.ToDto();
    }
    
    public JobDto GetJobById(GetJobByIdRequest request) {
        return JobRepository.FindById(request.Id)
            .OrElseThrow(() => new ArgumentException($"Job not found by id: {request.Id}"))
            .AssertCanRead(JobAuthorizer)
            .ToDto();
    }
    
    public JobDto UpdateJob(PatchJobRequest request) {
        var job = JobRepository.FindById(request.Id)
            .OrElseThrow(() => new ArgumentException($"Job not found by id: {request.Id}"));
        request.ApplyTo(job);
        return job.ToDto();
    }
}