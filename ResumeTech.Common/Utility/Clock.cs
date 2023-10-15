namespace ResumeTech.Common.Utility; 

public static class Clock {
    private static bool? canMockClock;
    private static DateTimeOffset? mockTime;
    public static DateTimeOffset? MockTime {
        get => mockTime;
        set {
            canMockClock ??= Environment.GetEnvironmentVariable("NoMockClock") == null;
            if (!canMockClock.Value) {
                throw new ArgumentException("Cannot mock clock in this environment");
            }
            mockTime = value;
        }
    }
    public static DateTimeOffset Now => MockTime ?? DateTimeOffset.Now;
}