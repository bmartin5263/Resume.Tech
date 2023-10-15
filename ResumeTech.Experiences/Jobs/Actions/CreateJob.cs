using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Common.Validation;
using ResumeTech.Experiences.Jobs.Dto;

namespace ResumeTech.Experiences.Jobs.Actions;

public class CreateJob : Command<CreateJobRequest, JobDto> {
    public override string Name => "CreateJob";

    private JobManager JobManager { get; }

    public CreateJob(JobManager jobManager) {
        JobManager = jobManager;
    }

    public override Task Validate(UserDetails user, CreateJobRequest args) {
        Validator<CreateJobRequest>.Create(args)
            .Check(v => v.CompanyName.AssertValid("companyName"))
            .CheckCollection("positions", r => r.Positions, positions => positions
                .Check(v => v.IsEmpty())
                .CheckEach(position => position
                    .Check(v => v.Title.AssertValid("title"))
                )
            )
            .ThrowIfFailed();
        
        return Task.CompletedTask;
    }

    public override Task<JobDto> Run(CreateJobRequest args) {
        return JobManager.CreateJob(args);
    }
}