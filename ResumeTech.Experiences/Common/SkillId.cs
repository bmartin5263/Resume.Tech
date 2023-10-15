using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Common;

public readonly record struct SkillId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static SkillId Generate() => new(Guid.NewGuid());
    public static SkillId Parse(string str) => new(Guid.Parse(str));
    public static readonly SkillId Empty = new();
    public override string ToString() => Value.ToString("N");
}