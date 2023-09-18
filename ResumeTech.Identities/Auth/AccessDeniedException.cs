using System.Net;
using ResumeTech.Common.Exceptions;

namespace ResumeTech.Identities.Auth; 

public class AccessDeniedException : AppException {
    public AccessDeniedException(string? DeveloperMessage = null) : base(new AppError(
        DeveloperMessage: DeveloperMessage,
        UserMessage: "User lacks sufficient privileges to perform the requested operation",
        StatusCode: HttpStatusCode.Forbidden
    )) {
    }
}