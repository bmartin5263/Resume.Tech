using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Users; 

public readonly record struct UserId(Guid Value) : IEntityId {
    public static UserId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}