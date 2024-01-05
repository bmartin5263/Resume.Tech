namespace ResumeTech.Common.Error; 

public class AppException : Exception {
    public AppError Error { get; }
    public AppException(AppError Error): base(Error.Message, Error.CausedBy) {
        this.Error = Error;
    }
}