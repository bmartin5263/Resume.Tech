using ResumeTech.Common.Dto;

namespace ResumeTech.Experiences.Jobs;

public sealed record PatchJobRequest {
    public JobId Id { get; }
    public RequiredFieldPatch<string>? Name { get; }
    
    public PatchJobRequest(
        JobId Id, 
        RequiredFieldPatch<string>? Name = default
    ) {
        this.Id = Id;
        this.Name = Name;
    }

    public void ApplyTo(Job job) {
        if (Name != null) {
            job.CompanyName = Name.Value.NewValue;
        }
    }
    
    public void Deconstruct(
        out JobId JobId,
        out RequiredFieldPatch<string>? Name
    ) {
        JobId = this.Id;
        Name = this.Name;
    }
}