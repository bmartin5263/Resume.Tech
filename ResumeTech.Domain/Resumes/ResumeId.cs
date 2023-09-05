using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Resumes;

public readonly record struct ResumeId(Guid Value) : IEntityId {
    public static ResumeId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}