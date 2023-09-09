namespace ResumeTech.ExperienceManagement.Domain;

public readonly record struct FilmId(Guid Value) : IProjectId {
    public static FilmId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}