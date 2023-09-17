using ResumeTech.Common.Cqs.Queries;

namespace ResumeTech.Experiences.Jobs.Cqs;

public class GetJobById : CqsQuery<GetJobByIdRequest, JobDto> {
    public override string Name => "GetJobById";
    
    private JobManager JobManager { get; }

    public GetJobById(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task<JobDto> Execute(GetJobByIdRequest args) {
        return JobManager.GetJobById(args);
    }
}