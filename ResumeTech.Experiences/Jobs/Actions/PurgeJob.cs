using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

public class PurgeJob : PureCommand<DeleteJobRequest> {
    public override string Name => "PurgeJob";
    public override Roles UserRoles => Roles.AdminOnly();
    
    private JobManager JobManager { get; }

    public PurgeJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task RunWithoutResult(DeleteJobRequest args) { 
        return JobManager.DeleteJob(args, purge: true);
    }
}