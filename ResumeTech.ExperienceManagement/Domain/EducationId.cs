using ResumeTech.Common.Domain;

namespace ResumeTech.ExperienceManagement.Domain;

public readonly record struct EducationId(Guid Value) : IEntityId {
    public static EducationId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}