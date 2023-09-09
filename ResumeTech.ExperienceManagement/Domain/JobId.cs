namespace ResumeTech.ExperienceManagement.Domain;

public readonly record struct JobId(Guid Value) : IProjectId {
    public static JobId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}