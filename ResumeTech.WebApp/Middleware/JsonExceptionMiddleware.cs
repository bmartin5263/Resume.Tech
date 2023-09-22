using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Json;

namespace ResumeTech.Application.Middleware; 

public class JsonExceptionMiddleware {
    private bool IsTestEnvironment { get; }
    private ILogger<JsonExceptionMiddleware> Logger { get; }

    public JsonExceptionMiddleware(bool isTestEnvironment) {
        IsTestEnvironment = isTestEnvironment;
        
        var factory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        Logger = factory.CreateLogger<JsonExceptionMiddleware>();
    }

    public async Task Invoke(HttpContext context) {
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        AppError error;
        Exception? thrownException = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        Exception? cause;
        switch (thrownException) {
            case null:
                // Should never get here
                return;
            case AppException e:
                error = e.Error;
                cause = e.InnerException;
                break;
            default:
                error = new AppError(CausedBy: thrownException);
                cause = thrownException;
                break;
        }
        
        int statusCode = (int) error.StatusCode;

        string? message;
        if (statusCode is >= 500 and <= 599) {
            message = cause?.ToString() ?? error.ToString();
            // Notify Admin
        }
        else {
            message = cause?.Message ?? error.DeveloperMessage ?? error.UserMessage;
        }
        
        if (message != null) {
            Logger.LogError(message);
        }

        bool isAdmin = context.User.IsInRole(RoleName.Admin.ToString());

        context.Response.StatusCode = (int) error.StatusCode;
        context.Response.ContentType = "application/json";

        await using var writer = new StreamWriter(context.Response.Body);
        var errorDto = error.ToDto(traceId: context.TraceIdentifier, includeDevInfo: isAdmin || IsTestEnvironment);
        await IJsonParser.Default.WriteAsync(writer.BaseStream, errorDto);
        await writer.FlushAsync().ConfigureAwait(false);
    }
}