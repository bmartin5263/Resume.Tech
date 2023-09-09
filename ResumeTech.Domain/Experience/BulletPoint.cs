using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Experience;

public record BulletPoint(string Value) : IWrapper<string> {
    private BulletPoint() : this(Value: null!) {}
}