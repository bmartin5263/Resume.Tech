using ResumeTech.Common.Actions;

namespace ResumeTech.Experiences.Jobs.Cqs;

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