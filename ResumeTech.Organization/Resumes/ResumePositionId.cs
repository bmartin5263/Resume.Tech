using ResumeTech.Common.Domain;

namespace ResumeTech.Organization.Resumes;

public readonly record struct ResumePositionId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static ResumePositionId Generate() => new(Guid.NewGuid());
    public static ResumePositionId Parse(string str) => new(Guid.Parse(str));
    public static readonly ResumePositionId Empty = new();
    public override string ToString() => Value.ToString("N");
}