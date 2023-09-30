using ResumeTech.Common.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

public class CreateJob : Command<CreateJobRequest, JobDto> {
    public override string Name => "CreateJob";

    private JobManager JobManager { get; }

    public CreateJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task<JobDto> Run(CreateJobRequest args) {
        return JobManager.CreateJob(args);
    }
}