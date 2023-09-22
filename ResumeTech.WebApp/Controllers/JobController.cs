using Microsoft.AspNetCore.Mvc;
using ResumeTech.Common.Cqs;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Cqs;

namespace ResumeTech.Application.Controllers;

[ApiController]
[Route("jobs")]
public class JobController : ControllerBase {
    private Exec Exec { get; }

    public JobController(Exec exec) {
        Exec = exec;
    }

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public Task<JobDto?> CreateJob([FromBody] CreateJobRequest request) {
        return Exec.Command<CreateJobRequest, JobDto>(request);
    }
    
    /// <summary>
    /// Find a job by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public Task<JobDto?> GetJobById(string id) {
        return Exec.Query<GetJobByIdRequest, JobDto>(new GetJobByIdRequest(
            Id: JobId.Parse(id))
        );
    }
    
    /// <summary>
    /// Update a Job
    /// </summary>
    [Route("{id}")]
    [HttpPatch]
    public Task<JobDto?> PatchJob(string id, [FromBody] PatchJobRequest request) {
        return Exec.Command<PatchJobRequest, JobDto>(request with {
            Id = JobId.Parse(id)
        });
    }
    
    /// <summary>
    /// Delete a Job
    /// </summary>
    [Route("{id}")]
    [HttpDelete]
    public Task DeleteJob(string id) {
        return Exec.Command(new DeleteJobRequest(Id: JobId.Parse(id)));
    }

}