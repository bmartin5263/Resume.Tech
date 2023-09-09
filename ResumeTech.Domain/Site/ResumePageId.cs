using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Site;

public readonly record struct ResumePageId(Guid Value) : IEntityId {
    public static ResumePageId Generate() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}