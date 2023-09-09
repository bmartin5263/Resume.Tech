using System.Net;
using ResumeTech.Common.Exceptions;

namespace ResumeTech.Common.Utility; 

public static class NullUtils {

    public static T OrElseThrow<T>(this T? self, Func<Exception> supplier) {
        if (self == null) {
            throw supplier();
        }
        return self;
    }

    public static T OrElseThrow<T>(this T? obj, string? DeveloperMessage, Exception? CausedBy = null, AppErrorType? ErrorType = null, HttpStatusCode? StatusCode = null, string? UserMessage = null, IList<AppSubError>? SubErrors = null) {
        if (obj == null) {
            throw new AppException(new AppError(CausedBy, ErrorType, StatusCode, UserMessage, DeveloperMessage, SubErrors));
        }
        return obj;
    }
}