namespace ResumeTech.Experiences.Common; 

public sealed record DateOnlyRange {
    public DateOnly? Start { get; }
    public DateOnly? End { get; }

    public DateOnlyRange() {
        Start = null;
        End = null;
    }

    public DateOnlyRange(DateOnly? Start, DateOnly? End = null) {
        this.Start = Start;
        this.End = End;
        Validate();
    }

    public DateOnlyRange(DateTimeOffset? Start, DateTimeOffset? End = null) {
        this.Start = Start.HasValue ? DateOnly.FromDateTime(Start.Value.Date) : null;
        this.End = End.HasValue ? DateOnly.FromDateTime(End.Value.Date) : null;;
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
        if (Start != null && End != null && Start > End) {
            throw new ArgumentException($"Start date {Start} cannot be after End date {End}");
        }
    }
}