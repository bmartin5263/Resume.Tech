using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.Resources.Domain;

public readonly record struct ImageRefId(Guid Value) : IEntityId {
    public static ImageRefId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}