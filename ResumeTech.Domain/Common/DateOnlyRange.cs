namespace ResumeTech.Domain.Common;

public readonly record struct DateOnlyRange(DateOnly Start, DateOnly? End) {
    public void validate() {
        if (End.HasValue && End.Value < Start) {
            throw new ArgumentException($"End date ({End}) cannot be before start ({Start})");
        }
    }
}