using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Site;

public readonly record struct IntroPageId(Guid Value) : IEntityId {
    public static IntroPageId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}