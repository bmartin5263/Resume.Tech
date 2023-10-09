using ResumeTech.Common.Domain;

namespace ResumeTech.Organization.Resumes;

public readonly record struct ResumeEducationId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static ResumeEducationId Generate() => new(Guid.NewGuid());
    public static ResumeEducationId Parse(string str) => new(Guid.Parse(str));
    public static readonly ResumeEducationId Empty = new();
    public override string ToString() => Value.ToString("N");
}