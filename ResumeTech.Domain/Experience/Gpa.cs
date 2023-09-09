namespace ResumeTech.Domain.Experience;

public readonly record struct Gpa {
    public decimal Value { get; }
    public decimal Max { get; }

    public Gpa(decimal value, decimal max) {
        Value = value;
        Max = max;
        validate();
    }

    public void validate() {
        if (Max <= 0) {
            throw new ArgumentException($"GPA Max ({Max}) cannot be less than or equal to 0");
        }
        if (Value < 0) {
            throw new ArgumentException($"GPA Value ({Value}) cannot be less than 0");
        }
        if (Value > Max) {
            throw new ArgumentException($"GPA Value ({Value}) cannot be greater than Max ({Max})");
        }
    }
}