using System.Net;
using ResumeTech.Common.Error;

namespace ResumeTech.Common.Utility; 

public static class ValidationUtils {

    public static string AssertMaxTrimmedLength(this string self, int max, string fieldName) {
        string trimmed = self.Trim();
        if (trimmed.Length == 0) {
            throw new AppError(UserMessage: $"{fieldName} cannot be blank").ToException();
        }
        if (trimmed.Length == 0 || trimmed.Length > max) {
            throw new AppError(UserMessage: $"{fieldName} cannot exceed {max} characters: {self}").ToException();
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