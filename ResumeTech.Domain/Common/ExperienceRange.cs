namespace ResumeTech.Domain.Common;

public readonly record struct ExperienceRange {
    public DateOnlyRange? DateRange { get; }
    public TimeSpan? Duration { get; }
    
    public ExperienceRange(DateOnlyRange? DateRange, TimeSpan? Duration) {
        this.DateRange = DateRange;
        this.Duration = Duration;
        validate();
    }

    public void validate() {
        if (DateRange.HasValue && Duration.HasValue) {
            throw new ArgumentException($"Cannot supply both a date range ({DateRange}) and duration ({Duration})");
        }
    }
    
    public void Deconstruct(out DateOnlyRange? DateRange, out TimeSpan? Duration) {
        DateRange = this.DateRange;
        Duration = this.Duration;
    }
}