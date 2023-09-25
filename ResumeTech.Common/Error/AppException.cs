namespace ResumeTech.Common.Error; 

public class AppException : Exception {
    public AppError Error { get; }

    public AppException(string? DeveloperMessage) : base(DeveloperMessage) {
        Error = new AppError(DeveloperMessage: DeveloperMessage);
    }

    public AppException(string? DeveloperMessage, Exception? CausedBy) : base(DeveloperMessage, CausedBy) {
        Error = new AppError(DeveloperMessage: DeveloperMessage, CausedBy: CausedBy);
    }

    public AppException(AppError Error): base(Error.UserMessage, Error.CausedBy) {
        this.Error = Error;
    }
}