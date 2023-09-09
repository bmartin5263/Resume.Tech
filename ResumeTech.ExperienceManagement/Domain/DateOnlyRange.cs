namespace ResumeTech.ExperienceManagement.Domain; 

public readonly record struct DateOnlyRange {
    public DateOnly? Start { get; }
    public DateOnly? End { get; }

    public DateOnlyRange(DateOnly Start, DateOnly End) {
        this.Start = Start;
        this.End = End;
    }

    public void Deconstruct(out DateOnly? Start, out DateOnly? End) {
        Start = this.Start;
        End = this.End;
    }
    
    public void Validate() {
        if (Start == null && End == null) {
            throw new ArgumentException("Missing both Start and End dates");
        }
        if (End.HasValue && !Start.HasValue) {
            throw new ArgumentException("Must provide and Start date when providing an End date");
        }
        if (Start > End.GetValueOrDefault(DateOnly.MaxValue)) {
            throw new ArgumentException($"Start date {Start} cannot be after End date {End}");
        }
    }
}