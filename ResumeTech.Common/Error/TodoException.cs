namespace ResumeTech.Common.Error; 

public class TodoException : Exception {
    public TodoException() { }
    public TodoException(string? message) : base(message) { }
    public TodoException(string? message, Exception? innerException) : base(message, innerException) { }
}