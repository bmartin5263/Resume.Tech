namespace ResumeTech.Domain.Common; 

public readonly record struct UserId(Guid Value) : IEntityId {
    public static UserId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}