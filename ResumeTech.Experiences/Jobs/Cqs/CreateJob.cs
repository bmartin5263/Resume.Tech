using ResumeTech.Common.Actions;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class CreateJob : Command<CreateJobRequest, JobDto> {
    public override string Name => "CreateJob";

    private JobManager JobManager { get; }

    public CreateJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task<JobDto> Run(CreateJobRequest args) {
        return Task.FromResult(JobManager.CreateJob(args));
    }
}