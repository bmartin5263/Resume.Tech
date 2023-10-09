using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Jobs;

public readonly record struct JobId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static JobId Generate() => new(Guid.NewGuid());
    public static JobId Parse(string str) => new(Guid.Parse(str));
    public static readonly JobId Empty = new();
    public override string ToString() => Value.ToString("N");
}