namespace ResumeTech.Common.Utility; 

public static class ValidationUtils {

    public static string AssertTrimmedMaxLength(this string self, int max, string fieldName) {
        string trimmed = self.Trim();
        if (trimmed.Length == 0) {
            throw new ArgumentException($"{fieldName} cannot be blank");
        }
        if (trimmed.Length == 0 || trimmed.Length > max) {
            throw new ArgumentException($"{fieldName} cannot exceed {max} characters: {self}");
        }
        return self;
    }
    
    public static string AssertLengthLessThanOrEqTo(this string self, int len, string fieldName) {
        if (self.Length > len) {
            throw new ArgumentException($"{fieldName} cannot exceed {len} characters: {self}");
        }
        return self;
    }

    public static decimal AssertPositiveOrZero(this decimal self, string fieldName) {
        if (self < 0) {
            throw new ArgumentException($"{fieldName} must be positive: {self}");
        }
        return self;
    }
    
    public static decimal AssertBetween(this decimal self, decimal min, decimal max, string fieldName) {
        if (self < min || self > max) {
            throw new ArgumentException($"{fieldName} must be between {min} and {max}: {self}");
        }
        return self;
    }

}