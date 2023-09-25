namespace ResumeTech.Experiences.Common; 

public sealed record DateOnlyRange {
    public DateOnly? Start { get; }
    public DateOnly? End { get; }

    private DateOnlyRange() {
        Start = null;
        End = null;
    }

    public DateOnlyRange(DateOnly? Start, DateOnly? End = null) {
        this.Start = Start;
        this.End = End;
        Validate();
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
        if (Start > (End ?? DateOnly.MaxValue)) {
            throw new ArgumentException($"Start date {Start} cannot be after End date {End}");
        }
    }
}