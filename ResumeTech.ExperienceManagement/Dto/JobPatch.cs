using ResumeTech.Common.Dto;
using ResumeTech.ExperienceManagement.Domain;

namespace ResumeTech.ExperienceManagement.Dto;

public sealed record JobPatch(
    RequiredFieldPatch<string>? Name = default
) {

    public void ApplyTo(Job job) {
        if (Name.HasValue) {
            job.CompanyName = Name.Value.NewValue;
        }
    }
    
}