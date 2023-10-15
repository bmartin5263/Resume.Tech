using ResumeTech.Common.Domain;

namespace ResumeTech.Organization.Resumes;

public readonly record struct ResumeBulletPointId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static ResumeBulletPointId Generate() => new(Guid.NewGuid());
    public static ResumeBulletPointId Parse(string str) => new(Guid.Parse(str));
    public static readonly ResumeBulletPointId Empty = new();
    public override string ToString() => Value.ToString("N");
}