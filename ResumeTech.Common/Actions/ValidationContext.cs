using System.Net;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions;

public record FieldError(string Path, string Message);
public record GeneralError(string Message);

public class StopValidationException : Exception {
    public StopValidationException() { }
}

public class ValidationFailedException : Exception {
    private ValidationError Error { get; }

    public ValidationFailedException(ValidationError error) {
        Error = error;
    }

    public AppError ToAppError() {
        return Error.ToAppError();
    }
}

public interface ValidationError {
    AppError ToAppError();
}

public class GeneralValidationError : ValidationError {
    public string Message { get; }

    public GeneralValidationError(string message) {
        Message = message;
    }

    public AppError ToAppError() {
        return new AppError(
            Message: Message,
            StatusCode: HttpStatusCode.BadRequest
        );
    }
}

public class FieldValidationErrors : ValidationError {
    public ISet<FieldError> FieldErrors { get; }

    public FieldValidationErrors() {
        FieldErrors = new HashSet<FieldError>();
    }

    public FieldValidationErrors(IEnumerable<FieldError> fieldErrors) {
        FieldErrors = fieldErrors.ToHashSet();
    }

    public AppError ToAppError() {
        if (FieldErrors.IsEmpty()) {
            throw new InvalidOperationException("Must have at least 1 Field Error");
        }
        return new AppError(
            Message: "Multiple validation errors occurred",
            StatusCode: HttpStatusCode.BadRequest,
            SubErrors: FieldErrors.Select(e => new AppSubError(e.Path, e.Message)).ToHashSet()
        );
    }
}

public class ValidationContext<T> {
    private T? Request { get; }
    public UserDetails User { get; }
    public ValidationError? Error { get; set; }
    public bool Failed => Error != null;

    public ValidationContext(T? request, UserDetails user) {
        Request = request;
        User = user;
    }

    public void AddError(string path, string message) {
        if (Error == null) {
            var fieldErrors = new FieldValidationErrors();
            fieldErrors.FieldErrors.Add(new FieldError(path, message));
            Error = fieldErrors;
        }
        else {
            if (Error is FieldValidationErrors fieldErrors) {
                fieldErrors.FieldErrors.Add(new FieldError(path, message));
            }
            else {
                throw new InvalidOperationException("Cannot have both field errors and a general error message");
            }
        }
    }

    public void ValidationFailed(string message) {
        if (Error != null) {
            throw new InvalidOperationException("Cannot have both field errors and a general error message");
        }
        Error = new GeneralValidationError(message);
        EndValidation();
    }

    public void EndValidation() {
        throw new StopValidationException();
    }

    public T GetRequest() {
        if (Request == null) {
            throw new InvalidOperationException("No request exists");
        }
        return Request!;
    }
}