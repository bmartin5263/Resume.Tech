using ResumeTech.Common.Auth;
using ResumeTech.Common.Cqs;
using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class PurgeJob : PureCqsCommand<DeleteJobRequest> {
    public override string Name => "PurgeJob";
    public override IReadOnlySet<RoleName> RequiresAnyRole => ReadOnly.SetOf(RoleName.Admin);
    private JobManager JobManager { get; }

    public PurgeJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task Execute(DeleteJobRequest args) { 
        return JobManager.PurgeJob(args);
    }
}