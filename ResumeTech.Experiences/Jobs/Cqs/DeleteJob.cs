using ResumeTech.Common.Cqs;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class DeleteJob : PureCqsCommand<DeleteJobRequest> {
    public override string Name => "DeleteJob";
    
    private JobManager JobManager { get; }

    public DeleteJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task Execute(DeleteJobRequest args) { 
        return JobManager.DeleteJob(args);
    }
}