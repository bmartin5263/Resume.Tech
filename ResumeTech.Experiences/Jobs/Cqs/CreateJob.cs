using ResumeTech.Common.Cqs;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class CreateJob : CqsCommand<CreateJobRequest, JobDto> {
    public override string Name => "CreateJob";
    public override bool AllowAnonymous => true;

    private JobManager JobManager { get; }

    public CreateJob(JobManager jobManager) {
        JobManager = jobManager;
        Console.WriteLine("New CreateJob");
    }

    public override Task<JobDto> Execute(CreateJobRequest args) {
        return Task.FromResult(JobManager.CreateJob(args));
    }
}