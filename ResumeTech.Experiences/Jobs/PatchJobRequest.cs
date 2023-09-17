using ResumeTech.Common.Dto;

namespace ResumeTech.Experiences.Jobs;

public sealed record PatchJobRequest(JobId Id, RequiredFieldPatch<string>? Name) {
    public void ApplyTo(Job job) {
        if (Name != null) {
            job.CompanyName = Name.Value.NewValue;
        }
    }
}