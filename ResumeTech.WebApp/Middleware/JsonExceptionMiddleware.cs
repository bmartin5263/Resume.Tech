using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Json;
using ResumeTech.Common.Utility;

namespace ResumeTech.Application.Middleware;

public class JsonExceptionMiddleware {
    private const string GenericErrorMsg =
        "A system error has occurred. Please try your request again in a few minutes or contact support";
    
    private bool IsTestEnvironment { get; }
    private ILogger Logger { get; }

    public JsonExceptionMiddleware(bool isTestEnvironment) {
        IsTestEnvironment = isTestEnvironment;
        Logger = Logging.CreateLogger<JsonExceptionMiddleware>();
    }

    public async Task Invoke(HttpContext context) {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        var thrownException = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (thrownException == null) {
            // Shouldn't get here, but if so just return
            return;
        }

        var error = ParseException(thrownException);

        int statusCode = (int) error.StatusCode;
        
        string? message = error.CausedBy?.ToString();
        if (message != null) {
            Logger.LogError(message);
        }

        bool isAdmin = context.User.IsInRole(RoleName.Admin.ToString());

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await using var writer = new StreamWriter(context.Response.Body);
        var errorDto = error.ToDto(traceId: context.TraceIdentifier, includeDevInfo: isAdmin || IsTestEnvironment);
        await IJsonParser.Default.WriteAsync(writer.BaseStream, errorDto);
        await writer.FlushAsync().ConfigureAwait(false);
    }

    private AppError ParseException(Exception ex) {
        return ex switch {
            UserException userException => new AppError(
                Message: ex.Message,
                StatusCode: userException.StatusCode ?? HttpStatusCode.BadRequest,
                CausedBy: ex
            ),
            ValidationFailedException validationFailed => validationFailed.ToAppError(),
            _ => new AppError(
                Message: GenericErrorMsg,
                StatusCode: HttpStatusCode.InternalServerError,
                CausedBy: ex
            )
        };
    }
}