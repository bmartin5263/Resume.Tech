namespace ResumeTech.Common.Error; 

/**
 * Exception for when a valid user may not perform a given operation
 */
public class AuthorizationException : Exception {
    public AuthorizationException() { }
    public AuthorizationException(string? message) : base(message) { }
    public AuthorizationException(string? message, Exception? innerException) : base(message, innerException) { }
}