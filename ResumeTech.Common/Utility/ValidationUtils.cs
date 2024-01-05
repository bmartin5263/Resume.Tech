using System.Net;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Error;

namespace ResumeTech.Common.Utility; 

public static class ValidationUtils {
    public const int DefaultMinFieldLen = 1;
    public const int DefaultMaxFieldLen = 255;

    public static string Validate(
        this string? self,
        string field,
        bool trim = true,
        int min = DefaultMinFieldLen,
        int max = DefaultMaxFieldLen
    ) {
        if (self == null) {
            throw new ArgumentException($"{field} is required");
        }
        if (trim) {
            self = self.Trim();
        }
        var len = self.Length;
        if (len > max) {
            throw new ArgumentException($"{field} cannot exceed {max} characters");
        }
        if (len < min) {
            throw new ArgumentException($"{field} must have at least {min} characters");
        }

        return self;
    }

    public static TWrapper Validate<TWrapper>(
        this TWrapper? self,
        string field,
        bool trim = true,
        int min = DefaultMinFieldLen,
        int max = DefaultMaxFieldLen
    ) where TWrapper : IWrapper<string> {
        if (self == null) {
            throw new ArgumentException($"{field} is required");
        }
        self.Value.Validate(field, trim, min, max);
        return self;
    }

    public static string? ValidateNullable(
        this string? self,
        string field,
        bool trim = true,
        int min = 1,
        int max = 255
    ) {
        if (self == null) {
            return self;
        }
        return self.Validate(field, trim, min, max);
    }
    
    public static TWrapper? ValidateNullable<TWrapper>(
        this TWrapper? self,
        string field,
        bool trim = true,
        int min = 1,
        int max = 255
    ) where TWrapper : IWrapper<string> {
        if (self == null) {
            return self;
        }
        return self.Validate(field, trim, min, max);
    }

    public static string AssertMaxTrimmedLength(this string self, int max, string fieldName) {
        string trimmed = self.Trim();
        if (trimmed.Length == 0) {
            throw new ArgumentException($"{fieldName} cannot be blank");
        }
        if (trimmed.Length == 0 || trimmed.Length > max) {
            throw new ArgumentException($"{fieldName} cannot exceed {max} characters");
        }
        return self;
    }
    
    public static IList<T> AssertNotEmpty<T>(this IList<T> self, string fieldName) {
        if (self.Count == 0) {
            throw new ArgumentException($"{fieldName} must not be empty");
        }
        return self;
    }
    
    public static IList<T> AssertMinSize<T>(this IList<T> self, int min, string fieldName) {
        if (self.Count < min) {
            throw new ArgumentException($"{fieldName} size must be at least {min}");
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