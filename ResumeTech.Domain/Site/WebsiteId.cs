using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Site;

public readonly record struct WebsiteId(Guid Value) : IEntityId {
    public static WebsiteId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}