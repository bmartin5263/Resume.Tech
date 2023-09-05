using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Experience;

public readonly record struct JobId(Guid Value) : IEntityId {
    public static JobId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}