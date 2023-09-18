using ResumeTech.Common.Utility;
using ResumeTech.Cqs;
using ResumeTech.Identities.Users;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class PurgeJob : PureCqsCommand<DeleteJobRequest> {
    public override string Name => "PurgeJob";
    public override IReadOnlySet<RoleName> AnyRole => ReadOnly.SetOf(RoleName.Admin);
    private JobManager JobManager { get; }

    public PurgeJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task Execute(DeleteJobRequest args) { 
        return JobManager.PurgeJob(args);
    }
}