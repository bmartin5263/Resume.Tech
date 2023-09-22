using System.Net;
using Microsoft.Extensions.Logging;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs; 

/**
 * Executes Commands and Queries with common operations, such as logging and event dispatching.
 */
public class Exec {
    private static readonly ILogger Log = Logging.CreateLogger<Exec>();

    private IUnitOfWork UnitOfWork { get; }
    private IEventDispatcher EventDispatcher { get; }
    private IUserDetailsProvider UserDetailsProvider { get; }
    // private IActionLogRepository ActionLogRepository { get; }

    public Exec(IUnitOfWork unitOfWork, IEventDispatcher eventDispatcher, IUserDetailsProvider userDetailsProvider) {
        UnitOfWork = unitOfWork;
        EventDispatcher = eventDispatcher;
        UserDetailsProvider = userDetailsProvider;
    }

    public Task<O?> Query<I, O>(I args) {
        var type = typeof(CqsQuery<,>).MakeGenericType(typeof(I), typeof(O));
        var query = (CqsQuery<I, O>) UnitOfWork.GetService(type);
        return RunQuery(query, args).Cast<O>();
    }

    public Task<O?> Query<O>() {
        var type = typeof(CqsQuery<>).MakeGenericType(typeof(O));
        var query = (CqsQuery<O>) UnitOfWork.GetService(type);
        return RunQuery(query, null).Cast<O>();
    }

    public Task<O?> Command<I, O>(I args) {
        var type = typeof(CqsCommand<,>).MakeGenericType(typeof(I), typeof(O));
        var command = (CqsCommand<I, O>) UnitOfWork.GetService(type);
        return ExecuteCommand(command, args).Cast<O>();
    }

    public Task<O?> Command<O>() {
        var type = typeof(CqsCommand<>).MakeGenericType(typeof(O));
        var command = (CqsCommand<O>) UnitOfWork.GetService(type);
        return ExecuteCommand(command, null).Cast<O>();
    }

    public Task Command<I>(I args) {
        var type = typeof(PureCqsCommand<>).MakeGenericType(typeof(I));
        var command = (PureCqsCommand<I>) UnitOfWork.GetService(type);
        return ExecuteCommand(command, args);
    }

    public Task Command() {
        var type = typeof(PureCqsCommand);
        var command = (PureCqsCommand) UnitOfWork.GetService(type);
        return ExecuteCommand(command, null);
    }

    public Task<object?> ExecuteCommand(CqsCommand command, object? args) {
        var logPolicy = command.LogPolicy;
        Log.LogInformation($"Executing Command {command.Name} with Log Policy {logPolicy}");
        return ExecuteCommandWithoutLogging(command, args);
    }

    private async Task<object?> ExecuteCommandWithoutLogging(CqsCommand command, object? args) {
        var userDetails = UserDetailsProvider.CurrentUser;
        var username = userDetails.Id?.Value.ToString("N") ?? "Anonymous";
        Log.LogInformation($"{username} is executing Command {command.Name}");

        if (userDetails.Id == null && !command.AllowAnonymous) {
            throw new AccessDeniedException(
                DeveloperMessage: "Not logged in",
                StatusCode: HttpStatusCode.Unauthorized
            );
        }

        var anyRoles = command.RequiresAnyRole;
        if (anyRoles.Count > 0 && anyRoles.Overlaps(userDetails.Roles)) {
            throw new AccessDeniedException(DeveloperMessage: $"Required Any Role: {anyRoles}");
        }
        
        var result = await command.Execute(args);
        var events = await UnitOfWork.Commit(); // saves db changes
        if (events.Count > 0) {
            DispatchEvents(events);
        }
        return result;
    }

    // private async Task<object?> ExecuteCommandWithLogging(CqsCommand command, object? args, bool includeArguments) {
    //     var commandName = command.Name;
    //     var actionLog = new ActionLog(
    //         name: commandName,
    //         type: ActionType.Command,
    //         arguments: includeArguments && args != null ? IJsonParser.Default.WriteJson(args) : null
    //     );
    //
    //     try {
    //         var result = await ExecuteCommand(command, args, actionLog.Id);
    //         await PersistActionLog(actionLog.Succeeded());
    //         return result;
    //     }
    //     catch (UserException e) {
    //         if (!e.Error.IsUserError) {
    //             await PersistActionLog(actionLog.Failed(e));
    //         }
    //         throw;
    //     }
    //     catch (Exception e) {
    //         await PersistActionLog(actionLog.Failed(e));
    //         throw;
    //     }
    // }
    

    public Task<object?> RunQuery(CqsQuery query, object? args) {
        var userDetails = UserDetailsProvider.CurrentUser;
        var username = userDetails.Id?.Value.ToString("N") ?? "Anonymous";
        Log.LogInformation($"{username} is executing Query {query.Name}");

        if (userDetails.Id == null && !query.AllowAnonymous) {
            throw new AccessDeniedException(
                DeveloperMessage: "Not logged in",
                StatusCode: HttpStatusCode.Unauthorized
            );
        }
        
        var anyRoles = query.Roles;
        if (anyRoles.Count > 0 && anyRoles.Overlaps(userDetails.Roles)) {
            throw new AccessDeniedException(DeveloperMessage: $"Requires Role: {anyRoles.ToExpandedString()}");
        }
        
        return query.Execute(args);
    }

    public Task EventHandler(DomainEventHandler handler, IDomainEvent domainEvent) {
        Log.LogInformation($"Executing Event Handler {handler.Name} for Event {domainEvent.Name}");
        return handler.OnEvent(domainEvent);
    }

    // private async Task RunEventHandlerWithLogging(DomainEventHandler handler, IDomainEvent domainEvent, bool includeEventDetails) {
    //     var actionLog = new ActionLog(
    //         name: handler.Name,
    //         type: ActionType.EventHandler,
    //         arguments: includeEventDetails ? IJsonParser.Default.WriteJson(domainEvent) : null
    //     );
    //
    //     try {
    //         await handler.OnEvent(domainEvent);
    //     }
    //     catch (UserException e) {
    //         if (!e.Error.IsUserError) {
    //             await PersistActionLog(actionLog.Failed(e));
    //         }
    //     }
    //     catch (Exception e) {
    //         await PersistActionLog(actionLog.Failed(e));
    //     }
    //
    //     await PersistActionLog(actionLog.Succeeded());
    // }

    private void DispatchEvents(IEnumerable<IDomainEvent> domainEvents) {
        Task.Run(async () => {
            try {
                await EventDispatcher.Dispatch(domainEvents);
            }
            catch (Exception e) {
                Log.LogError($"Failed to dispatch domain events: {e}");
            }
        });
    }

    // private Task PersistActionLog(ActionLog actionLog) {
    //     try {
    //         ActionLogRepository.Add(actionLog);
    //         return UnitOfWork.SaveDbChanges();
    //     }
    //     catch (Exception) {
    //         // Abandon trying to save this log
    //         Log.Error("Failed to save action log");
    //         return Task.CompletedTask;
    //     }
    // }
}

// public static class ExecUtil {
//     
//     public static Task<O> Execute<I, O>(this IAppServiceProvider serviceProvider, CqsCommand<I, O> command, I args) {
//         return serviceProvider.GetService<Exec>().Command(command, args);
//     }
//     
//     public static Task<O> Execute2<I, O>(this IAppServiceProvider serviceProvider, I args) 
//     {
//         var command = serviceProvider.GetService<CqsCommand<I, O>>();
//         return serviceProvider.GetService<Exec>().Command(command, args);
//     }
//     
//     public static Task<O> Execute<O>(this IAppServiceProvider serviceProvider, CqsCommand<O> command) {
//         return serviceProvider.GetService<Exec>().Command(command);
//     }
//     
//     public static Task Execute<I>(this IAppServiceProvider serviceProvider, PureCqsCommand<I> command, I args) {
//         return serviceProvider.GetService<Exec>().Command(command, args);
//     }
//
//     public static Task Execute(this IAppServiceProvider serviceProvider, PureCqsCommand command) {
//         return serviceProvider.GetService<Exec>().Command(command);
//     }
//     
// }