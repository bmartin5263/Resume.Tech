using ResumeTech.Common.Domain;

namespace ResumeTech.ExperienceManagement.Domain;

public record BulletPoint(string Value) : IWrapper<string> {
    private BulletPoint() : this(Value: null!) {}
}