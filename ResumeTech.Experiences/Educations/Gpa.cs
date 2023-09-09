using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Educations;

public readonly record struct Gpa {
    public decimal Value { get; init; }
    public decimal Scale { get; init; }

    public Gpa(decimal Value, decimal Scale) {
        this.Value = Value.AssertBetween(0, Scale, "GPA Value");
        this.Scale = Scale.AssertPositiveOrZero("GPA Scale");
    }
    
    public void Deconstruct(out decimal Value, out decimal Scale) {
        Value = this.Value;
        Scale = this.Scale;
    }
}