using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Portfolios;

public readonly record struct PortfolioId(Guid Value) : IEntityId {
    public static PortfolioId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}