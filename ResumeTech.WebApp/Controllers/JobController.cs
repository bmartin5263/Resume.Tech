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
    /// Create a new Job
    /// </summary>
    [Route("{id}")]
    [HttpGet]
    public Task<JobDto> GetJobById(string id, [FromQuery] string userId) {
        ServiceProvider.GetRequiredService<UserIdProvider>().Set(new UserId(Guid.Parse(userId)));
        return ServiceProvider.GetRequiredService<GetJobById>()
            .Execute(new GetJobByIdRequest(
                Id: JobId.Parse(id)
            ));
    }
    
    /// <summary>
    /// Create a new Job
    /// </summary>
    [Route("")]
    [HttpPatch]
    public Task<JobDto> PatchJob([FromBody] PatchJobRequest request) {
        return ServiceProvider.GetRequiredService<PatchJob>()
            .Execute(request);
    }
    
}