using System.Collections.Immutable;
using System.Net;
using System.Text.Json.Serialization;
using ResumeTech.Common.Utility;
using static System.Text.Json.Serialization.JsonIgnoreCondition;

namespace ResumeTech.Common.Error;

public sealed record AppSubError(string Path, string Message);

public sealed record AppError {
    public string Message { get; }
    public Exception? CausedBy { get; }
    public ISet<AppSubError> SubErrors { get; }
    public HttpStatusCode StatusCode { get; }
    
    public AppError(
        string Message, 
        HttpStatusCode StatusCode,
        Exception? CausedBy = null, 
        ISet<AppSubError>? SubErrors = null
    ) {
        this.Message = Message;
        this.StatusCode = StatusCode;
        this.CausedBy = CausedBy;
        this.SubErrors = SubErrors ?? ImmutableHashSet<AppSubError>.Empty;
    }

    public AppErrorDto ToDto(string traceId, bool includeDevInfo) {
        return new AppErrorDto(
            CausedBy: includeDevInfo ? CausedBy?.GetType().Name : null,
            Message: Message,
            SubErrors: SubErrors.Count == 0 ? null : SubErrors.Select(e => new AppSubErrorDto(
                Path: e.Path,
                Message: e.Message
            )).ToList(),
            TraceId: traceId
        );
    }

    public AppException ToException() {
        return new AppException(this);
    }
    
    public static AppErrorBuilder Builder(string message) {
        return new AppErrorBuilder(message);
    }
}

public class AppErrorBuilder {
    private string _message;
    private Exception? _causedBy;
    private HashSet<AppSubError>? _subErrors;
    private HttpStatusCode _statusCode;

    public AppErrorBuilder(string message) {
        _message = message;
    }

    public AppErrorBuilder CausedBy(Exception exception) {
        _causedBy = exception;
        return this;
    }

    public AppErrorBuilder SubError(string path, string message) {
        _subErrors ??= new HashSet<AppSubError>();
        _subErrors.Add(new AppSubError(path, message));
        return this;
    }
    
    public AppErrorBuilder SubError(AppSubError subError) {
        _subErrors ??= new HashSet<AppSubError>();
        _subErrors.Add(subError);
        return this;
    }

    public AppErrorBuilder SubErrors(IEnumerable<AppSubError> subErrors) {
        _subErrors ??= new HashSet<AppSubError>();
        _subErrors.UnionWith(subErrors);
        return this;
    }

    public AppError Build() {
        return new AppError(
            Message: _message,
            CausedBy: _causedBy,
            SubErrors: _subErrors?.ToImmutableHashSet(),
            StatusCode: _statusCode
        );
    }

    public AppException ToException() {
        return new AppException(Build());
    }
}

public sealed record AppSubErrorDto(
    [property: JsonIgnore(Condition = WhenWritingNull)] string? Path = null,
    [property: JsonIgnore(Condition = WhenWritingNull)] string? Message = null
);

public sealed record AppErrorDto(
    [property: JsonIgnore(Condition = WhenWritingNull)] string? CausedBy = null,
    [property: JsonIgnore(Condition = WhenWritingNull)] string? Message = null,
    [property: JsonIgnore(Condition = WhenWritingNull)] List<AppSubErrorDto>? SubErrors = null,
    [property: JsonIgnore(Condition = WhenWritingNull)] HttpStatusCode? StatusCode = null,
    [property: JsonIgnore(Condition = WhenWritingNull)] string? TraceId = null
);