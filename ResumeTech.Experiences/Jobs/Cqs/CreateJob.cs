using ResumeTech.Common.Cqs.Commands;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class CreateJob : CqsCommand<CreateJobRequest, JobDto> {
    public override string Name => "CreateJob";
    
    private JobManager JobManager { get; }

    public CreateJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task<JobDto> Execute(CreateJobRequest args) {
        return Task.FromResult(JobManager.CreateJob(args));
    }
}