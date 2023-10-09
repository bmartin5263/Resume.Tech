using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Jobs;

public readonly record struct PositionId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static PositionId Generate() => new(Guid.NewGuid());
    public static PositionId Parse(string str) => new(Guid.Parse(str));
    public static readonly PositionId Empty = new();
    public override string ToString() => Value.ToString("N");
}