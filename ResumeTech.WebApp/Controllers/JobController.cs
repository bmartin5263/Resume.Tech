using Microsoft.AspNetCore.Mvc;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Cqs;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Application.Controllers;

[ApiController]
[Route("jobs")]
public class JobController : ControllerBase {
    private IServiceProvider ServiceProvider { get; }

    public JobController(IServiceProvider serviceProvider) {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public Task<JobDto> CreateJob([FromBody] CreateJobRequest request) {
        return ServiceProvider.GetRequiredService<CreateJob>()
            .Execute(request);
    }
    
    /// <summary>
    /// Find a job by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public Task<JobDto> GetJobById(string id) {
        return ServiceProvider.GetRequiredService<GetJobById>()
            .Execute(new GetJobByIdRequest(Id: JobId.Parse(id)));
    }
    
    /// <summary>
    /// Update a Job
    /// </summary>
    [Route("{id}")]
    [HttpPatch]
    public Task<JobDto> PatchJob(string id, [FromBody] PatchJobRequest request) {
        return ServiceProvider.GetRequiredService<PatchJob>()
            .Execute(request with {Id = JobId.Parse(id)});
    }
    
    /// <summary>
    /// Delete a Job
    /// </summary>
    [Route("{id}")]
    [HttpDelete]
    public Task DeleteJob(string id) {
        return ServiceProvider.GetRequiredService<DeleteJob>()
            .Execute(new DeleteJobRequest(Id: JobId.Parse(id)));
    }

}