using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class PurgeJob : PureCommand<DeleteJobRequest> {
    public override string Name => "PurgeJob";
    public override RoleName[] Roles { get; } = { RoleName.Admin };
    
    private JobManager JobManager { get; }

    public PurgeJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task RunWithoutResult(DeleteJobRequest args) { 
        return JobManager.PurgeJob(args);
    }
}