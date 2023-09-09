using ResumeTech.Common.Dto;
using ResumeTech.Identities.Domain;

namespace ResumeTech.Experiences.Jobs;

public sealed record JobPatch(
    UserId UserId,
    RequiredFieldPatch<string>? Name = default
) {

    public void ApplyTo(Job job) {
        if (Name != null) {
            job.CompanyName = Name.Value.NewValue;
        }
    }
    
}