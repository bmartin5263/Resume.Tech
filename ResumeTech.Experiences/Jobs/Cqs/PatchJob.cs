using ResumeTech.Common.Cqs;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class PatchJob : CqsCommand<PatchJobRequest, JobDto> {
    public override string Name => "PatchJob";
    
    private JobManager JobManager { get; }

    public PatchJob(JobManager jobManager) {
        JobManager = jobManager;
        Console.WriteLine("New PatchJob");
    }

    public override Task<JobDto> Execute(PatchJobRequest args) {
        return JobManager.UpdateJob(args);
    }
}