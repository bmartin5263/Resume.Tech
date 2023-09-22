using System.Net;
using ResumeTech.Common.Exceptions;

namespace ResumeTech.Common.Auth; 

public class AccessDeniedException : AppException {
    public AccessDeniedException(string? DeveloperMessage = null, HttpStatusCode StatusCode = HttpStatusCode.Forbidden) 
        : base(new AppError(
            DeveloperMessage: DeveloperMessage,
            UserMessage: "User lacks sufficient privileges to perform the requested operation",
            StatusCode: StatusCode
        )) {
    }
}