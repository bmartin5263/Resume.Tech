using ResumeTech.Common.Domain;

namespace ResumeTech.Organization.Resumes;

public readonly record struct ResumeJobId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static ResumeJobId Generate() => new(Guid.NewGuid());
    public static ResumeJobId Parse(string str) => new(Guid.Parse(str));
    public static readonly ResumeJobId Empty = new();
    public override string ToString() => Value.ToString("N");
}