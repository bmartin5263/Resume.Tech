using Microsoft.AspNetCore.Mvc;
using ResumeTech.Common.Service;
using ResumeTech.Cqs;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Cqs;
using ResumeTech.Identities.Users;

namespace ResumeTech.Application.Controllers;

[ApiController]
[Route("jobs")]
public class JobController : ControllerBase {
    private IScope Scope { get; }
    private Exec Exec { get; }

    public JobController(IScope appServiceProvider, Exec exec) {
        Scope = appServiceProvider;
        Exec = exec;
    }

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public Task<JobDto> CreateJob([FromBody] CreateJobRequest request) {
        return Exec.Command(Scope.GetService<CreateJob>(), request, UserDetails.NotLoggedIn);
    }
    
    /// <summary>
    /// Find a job by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public Task<JobDto> GetJobById(string id) {
        return Exec.Query(Scope.GetService<GetJobById>(), new GetJobByIdRequest(Id: JobId.Parse(id)), UserDetails.NotLoggedIn);
    }
    
    /// <summary>
    /// Update a Job
    /// </summary>
    [Route("{id}")]
    [HttpPatch]
    public Task<JobDto> PatchJob(string id, [FromBody] PatchJobRequest request) {
        return Exec.Command(Scope.GetService<PatchJob>(), request with {Id = JobId.Parse(id)}, UserDetails.NotLoggedIn);
    }
    
    /// <summary>
    /// Delete a Job
    /// </summary>
    [Route("{id}")]
    [HttpDelete]
    public Task DeleteJob(string id) {
        return Exec.Command(Scope.GetService<DeleteJob>(), new DeleteJobRequest(Id: JobId.Parse(id)), UserDetails.NotLoggedIn);
    }

}