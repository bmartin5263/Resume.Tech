using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Site;

public readonly record struct PortfolioPageId(Guid Value) : IEntityId {
    public static PortfolioPageId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}