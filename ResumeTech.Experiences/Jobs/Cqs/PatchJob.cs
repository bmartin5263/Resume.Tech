using ResumeTech.Common.Actions;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class PatchJob : Command<PatchJobRequest, JobDto> {
    public override string Name => "PatchJob";
    
    private JobManager JobManager { get; }

    public PatchJob(JobManager jobManager) {
        JobManager = jobManager;
        Console.WriteLine("New PatchJob");
    }

    public override Task<JobDto> Run(PatchJobRequest args) {
        return JobManager.UpdateJob(args);
    }
}