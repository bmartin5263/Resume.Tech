using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Common;

public readonly record struct BulletPointId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static BulletPointId Generate() => new(Guid.NewGuid());
    public static BulletPointId Parse(string str) => new(Guid.Parse(str));
    public static readonly BulletPointId Empty = new();
    public override string ToString() => Value.ToString("N");
}