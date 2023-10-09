using ResumeTech.Common.Domain;

namespace ResumeTech.Organization.Resumes;

public readonly record struct ResumeId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static ResumeId Generate() => new(Guid.NewGuid());
    public static ResumeId Parse(string str) => new(Guid.Parse(str));
    public static readonly ResumeId Empty = new();
    public override string ToString() => Value.ToString("N");
}