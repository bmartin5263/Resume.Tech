namespace ResumeTech.Common.Domain;

public readonly record struct TagId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static TagId Generate() => new(Guid.NewGuid());
    public static TagId Parse(string str) => new(Guid.Parse(str));
    public static readonly TagId Empty = new();
    public override string ToString() => Value.ToString("N");
}