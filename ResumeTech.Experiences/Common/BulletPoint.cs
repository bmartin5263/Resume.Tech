using ResumeTech.Common.Domain;

namespace ResumeTech.Experiences.Common;

public sealed record BulletPoint(string Value) : IWrapper<string> {
    private BulletPoint() : this(Value: null!) {}
}