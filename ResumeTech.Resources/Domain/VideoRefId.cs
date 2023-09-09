using ResumeTech.Common;
using ResumeTech.Common.Domain;

namespace ResumeTech.Resources.Domain;

public readonly record struct VideoRefId(Guid Value) : IEntityId {
    public static VideoRefId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}