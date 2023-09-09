using ResumeTech.Common.Utility;
using ResumeTech.ExperienceManagement.Domain;
using ResumeTech.ExperienceManagement.Dto;
using ResumeTech.ExperienceManagement.Repository;

namespace ResumeTech.ExperienceManagement.Service; 

public class JobManager {
    private IJobRepository JobRepository { get; }
    
    public JobManager(IJobRepository jobRepository) {
        JobRepository = jobRepository;
    }

    public JobDto CreateJob(CreateJobRequest request) {
        var job = new Job(Name: request.Name);
        JobRepository.Add(job);
        return job.ToDto();
    }
    
    public JobDto UpdateJob(JobId id, JobPatch patch) {
        var job = JobRepository.FindById(id).OrElseThrow(() => new ArgumentException($"Job not found by id: {id}"));
        patch.ApplyTo(job);
        return job.ToDto();
    }
}