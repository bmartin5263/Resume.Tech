using ResumeTech.Common.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

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