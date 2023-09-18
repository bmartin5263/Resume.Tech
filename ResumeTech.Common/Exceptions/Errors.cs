using System.Net;

namespace ResumeTech.Common.Exceptions; 

public static class Errors {
    public static AppErrorBuilder EntityNotFound<T>(object id) {
        return Builder(HttpStatusCode.BadRequest)
            .UserMessage($"No {typeof(T).Name} found with id {id}");
    }
    
    public static AppErrorBuilder EntityNotFound(string name, object id) {
        return Builder(HttpStatusCode.BadRequest)
            .UserMessage($"No {name} found with id {id}");
    }
    
    public static AppErrorBuilder EntityNotFound<T>(string fieldName, object value) {
        return Builder(HttpStatusCode.BadRequest)
            .UserMessage($"No {typeof(T).Name} found with {fieldName} {value}");
    }
    
    public static AppErrorBuilder EntityMissing<T>(object id, string? message = null) {
        return Builder(HttpStatusCode.InternalServerError)
            .DeveloperMessage($"Missing {typeof(T).Name} with id {id}. {message}");
    }
    
    public static AppErrorBuilder Unauthorized(string message = "Login failed. Reset password at https://User.io/reset-password") {
        return Builder(HttpStatusCode.Unauthorized)
            .UserMessage(message);
    }
    
    public static AppErrorBuilder Builder(HttpStatusCode statusCode) {
        return new AppErrorBuilder()
            .StatusCode(statusCode);
    }
    
    public static AppException Wrap(Exception e, string? userMessage = null) {
        return new AppErrorBuilder()
            .CausedBy(e)
            .UserMessage(userMessage)
            .StatusCode(HttpStatusCode.InternalServerError)
            .ToException();
    }
}