using ResumeTech.Common.Domain;

namespace ResumeTech.Common.Auth;

public readonly record struct UserId(Guid Value) : IEntityId, IWrapper<Guid> {
    public static UserId Generate() => new(Guid.NewGuid());
    public static UserId Parse(string str) => new(Guid.Parse(str));
    public override string ToString() => Value.ToString("N");
}