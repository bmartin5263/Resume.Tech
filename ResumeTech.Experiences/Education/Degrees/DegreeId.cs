using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Education.Degrees;

public readonly record struct DegreeId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static DegreeId Generate() => new(Guid.NewGuid());
    public static DegreeId Parse(string str) => new(Guid.Parse(str));
    public static readonly DegreeId Empty = new();
    public override string ToString() => Value.ToString("N");
}