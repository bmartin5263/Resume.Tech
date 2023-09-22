using Microsoft.AspNetCore.Mvc;
using ResumeTech.Common.Cqs;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Cqs;

namespace ResumeTech.Application.Controllers;

[ApiController]
[Route("jobs")]
public class JobController : ControllerBase {
    private IUnitOfWork UnitOfWork { get; }
    private Exec Exec { get; }

    public JobController(IUnitOfWork appServiceProvider, Exec exec) {
        UnitOfWork = appServiceProvider;
        Exec = exec;
    }

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public Task<JobDto> CreateJob([FromBody] CreateJobRequest request) {
        return Exec.Command(UnitOfWork.GetService<CreateJob>(), request);
    }
    
    /// <summary>
    /// Find a job by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public Task<JobDto> GetJobById(string id) {
        return Exec.Query(UnitOfWork.GetService<GetJobById>(), new GetJobByIdRequest(Id: JobId.Parse(id)));
    }
    
    /// <summary>
    /// Update a Job
    /// </summary>
    [Route("{id}")]
    [HttpPatch]
    public Task<JobDto> PatchJob(string id, [FromBody] PatchJobRequest request) {
        return Exec.Command(UnitOfWork.GetService<PatchJob>(), request with {Id = JobId.Parse(id)});
    }
    
    /// <summary>
    /// Delete a Job
    /// </summary>
    [Route("{id}")]
    [HttpDelete]
    public Task DeleteJob(string id) {
        return Exec.Command(UnitOfWork.GetService<DeleteJob>(), new DeleteJobRequest(Id: JobId.Parse(id)));
    }

}