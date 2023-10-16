using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTech.Application.Util;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Application.Controllers;

[Authorize]
[ApiController]
[Route("jobs")]
public class JobController : ControllerBase {
    private Exec Exec { get; }
    private IUnitOfWork UnitOfWork { get; }
    private IUserProvider UserProvider { get; }

    public JobController(Exec exec, IUnitOfWork unitOfWork, IUserProvider userProvider) {
        Exec = exec;
        UnitOfWork = unitOfWork;
        UserProvider = userProvider;
    }

    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPost]
    public Task<JobDto> CreateJob([FromBody] CreateJobRequest request) {
        var command = UnitOfWork.GetService<CreateJob>();
        return Exec.Command(command, UserProvider.CurrentUser, request);
    }
    
    /// <summary>
    /// Find a job by Id
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public async Task<JobDto> GetJobById(string id) {
        var jobId = JobId.Parse(id);
        var query = UnitOfWork.GetService<GetJobById>();
        var result = await Exec.Query(query, UserProvider.CurrentUser, new GetJobByIdRequest(
            Id: JobId.Parse(id))
        );
        return result.OrElseNotFound(jobId);
    }
    
    /// <summary>
    /// Update a Job
    /// </summary>
    [Route("{id}")]
    [HttpPatch]
    public Task<JobDto> PatchJob(string id, [FromBody] PatchJobRequest request) {
        var command = UnitOfWork.GetService<PatchJob>();
        return Exec.Command(command, UserProvider.CurrentUser, request with {
            Id = JobId.Parse(id)
        });
    }
    
    /// <summary>
    /// Delete a Job
    /// </summary>
    [Route("{id}")]
    [HttpDelete]
    public Task DeleteJob(string id) {
        var command = UnitOfWork.GetService<DeleteJob>();
        return Exec.Command(command, UserProvider.CurrentUser, new DeleteJobRequest(Id: JobId.Parse(id)));
    }

    /// <summary>
    /// Purge a Job
    /// </summary>
    [Route("{id}/purge")]
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public Task PurgeJob(string id) {
        var command = UnitOfWork.GetService<PurgeJob>();
        return Exec.Command(command, UserProvider.CurrentUser, new DeleteJobRequest(Id: JobId.Parse(id)));
    }

}