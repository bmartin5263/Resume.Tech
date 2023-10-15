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
            throw new AppError(UserMessage: $"{field} is required").ToException();
        }
        if (trim) {
            self = self.Trim();
        }
        var len = self.Length;
        if (len > max) {
            throw new AppError(UserMessage: $"{field} cannot exceed {max} characters").ToException();
        }
        if (len < min) {
            throw new AppError(UserMessage: $"{field} must have at least {min} characters").ToException();
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
            throw new AppError(UserMessage: $"{field} is required").ToException();
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
            throw new AppError(UserMessage: $"{fieldName} cannot be blank").ToException();
        }
        if (trimmed.Length == 0 || trimmed.Length > max) {
            throw new AppError(UserMessage: $"{fieldName} cannot exceed {max} characters").ToException();
        }
        return self;
    }
    
    public static IList<T> AssertNotEmpty<T>(this IList<T> self, string fieldName) {
        if (self.Count == 0) {
            throw AppError.Builder(HttpStatusCode.BadRequest)
                .SubError(fieldName, "Must not be empty")
                .ToException();
        }
        return self;
    }
    
    public static IList<T> AssertMinSize<T>(this IList<T> self, int min, string fieldName) {
        if (self.Count < min) {
            throw AppError.Builder(HttpStatusCode.BadRequest)
                .SubError(fieldName, $"Size must be at least {min}")
                .ToException();
        }
        return self;
    }

    public static decimal AssertPositiveOrZero(this decimal self, string fieldName) {
        if (self < 0) {
            throw new AppError(UserMessage: $"{fieldName} must be positive: {self}").ToException();
        }
        return self;
    }
    
    public static decimal AssertBetween(this decimal self, decimal min, decimal max, string fieldName) {
        if (self < min || self > max) {
            throw new AppError(UserMessage: $"{fieldName} must be between {min} and {max}: {self}").ToException();
        }
        return self;
    }

}