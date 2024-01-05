using System.Net;

namespace ResumeTech.Common.Error; 

public class UserException : Exception {
    public HttpStatusCode? StatusCode { get; }
    
    public UserException(Exception CausedBy) : base(CausedBy.Message, CausedBy) {
    }

    public UserException(Exception CausedBy, HttpStatusCode statusCode) : base(CausedBy.Message, CausedBy) {
        StatusCode = statusCode;
    }
}