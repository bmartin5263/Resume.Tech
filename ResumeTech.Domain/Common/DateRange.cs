namespace ResumeTech.Domain.Common; 

public readonly record struct DateRange {
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    public TimeSpan Duration => End - Start;

    public DateRange(DateTimeOffset start, DateTimeOffset end) {
        Start = start;
        End = end;
        validate();
    }

    public void validate() {
        if (Start > End) {
            throw new ArgumentException($"DateRange start ({Start}) cannot be after end ({End})");
        }
    }
}