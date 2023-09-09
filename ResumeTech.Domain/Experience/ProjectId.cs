using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Experience;

public readonly record struct ProjectId(Guid Value) : IEntityId {
    public static ProjectId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}