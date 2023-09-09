namespace ResumeTech.ExperienceManagement.Domain;

public readonly record struct SoftwareId(Guid Value) : IProjectId {
    public static SoftwareId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}