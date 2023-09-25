using ResumeTech.Common.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

public class DeleteJob : PureCommand<DeleteJobRequest> {
    public override string Name => "DeleteJob";
    
    private JobManager JobManager { get; }

    public DeleteJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task RunWithoutResult(DeleteJobRequest args) { 
        return JobManager.DeleteJob(args);
    }
}