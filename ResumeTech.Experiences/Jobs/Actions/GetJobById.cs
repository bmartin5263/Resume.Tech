using ResumeTech.Common.Actions;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

public class GetJobById : Query<GetJobByIdRequest, JobDto?> {
    public override string Name => "GetJobById";
    
    private JobManager JobManager { get; }

    public GetJobById(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task<JobDto?> Run(GetJobByIdRequest args) {
        return JobManager.GetJobById(args);
    }
}