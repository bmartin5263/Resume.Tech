namespace ResumeTech.Common.Exceptions; 

public class AppException : Exception {
    public AppError Error { get; }

    public AppException(string? message) : base(message) {
        Error = new AppError(DeveloperMessage: message);
    }

    public AppException(string? message, Exception? innerException) : base(message, innerException) {
        Error = new AppError(DeveloperMessage: message, CausedBy: innerException);
    }

    public AppException(AppError error): base(error.UserMessage, error.CausedBy) {
        Error = error;
    }
}