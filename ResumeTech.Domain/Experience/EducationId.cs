using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Experience;

public readonly record struct EducationId(Guid Value) : IEntityId {
    public static EducationId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}