using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Contacts;

public readonly record struct ContactInfoId(Guid Value) : IEntityId {
    public static ContactInfoId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}