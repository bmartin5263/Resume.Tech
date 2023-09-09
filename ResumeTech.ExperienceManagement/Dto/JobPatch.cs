using ResumeTech.Common.Dto;
using ResumeTech.ExperienceManagement.Domain;

namespace ResumeTech.ExperienceManagement.Dto;

public sealed record JobPatch(
    RequiredFieldPatch<string>? Name = default,
    RequiredFieldPatch<DateOnlyRange>? Dates = default
) {

    public void ApplyTo(Job job) {
        if (Name.HasValue) {
            job.Name = Name.Value.NewValue;
        }
        if (Dates.HasValue) {
            job.Dates = Dates.Value.NewValue;
        }
    }
    
}