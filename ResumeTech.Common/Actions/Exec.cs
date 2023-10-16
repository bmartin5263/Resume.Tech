using System.Net;
using Microsoft.Extensions.Logging;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions;

/**
 * Executes Commands and Queries with common operations, such as logging and event dispatching.
 */
public class Exec : IExec {
    private static readonly ILogger Log = Logging.CreateLogger<Exec>();

    private IUnitOfWork UnitOfWork { get; }
    private IEventDispatcher EventDispatcher { get; }
    // private IActionLogRepository ActionLogRepository { get; }

    public Exec(IUnitOfWork unitOfWork, IEventDispatcher eventDispatcher) {
        UnitOfWork = unitOfWork;
        EventDispatcher = eventDispatcher;
    }
    
    // Tell compiler to ignore nullability generic types for these function calls
#pragma warning disable CS8619
    public Task<O> Command<I, O>(Command<I, O> command, I args) where O : notnull {
        return RunCommand(command!, args);
    }

    public Task<O> Command<O>(Command<O> command) where O : notnull {
        return RunCommand(command!, null);
    }

    public Task Command<I>(PureCommand<I> command, I args) {
        return RunCommand(command!, args);
    }
    
    public Task Command(PureCommand command) {
        return RunCommand(command!, null);
    }

    public Task<O> Query<I, O>(Query<I, O> command, I args){
        return RunQuery(command!, args);
    }

    public Task<O> Query<O>(Query<O> command) {
        return RunQuery(command!, null);
    }
#pragma warning restore CS8619
    
    public Task<O?> RunCommand<I, O>(Command<I?, O?> command, I? args) {
        var logPolicy = command.LogPolicy;
        Log.LogInformation("Executing Command {CommandName} with Log Policy {LogPolicy}", command.Name, logPolicy);
        return RunCommandWithoutLogging(command, args);
    }

    private static void Authenticate<I, O>(Action<I?, O?> command, UserDetails user) {
        if (!command.RequiresLoggedInUser || user.IsAdmin()) {
            return;
        }
        
        if (user.Id == null) {
            throw new AccessDeniedException("Not logged in", HttpStatusCode.Unauthorized);
        }
        
        command.UserRoles.Authorize(user);
    }
    
    private async Task<O?> RunCommandWithoutLogging<I, O>(Command<I?, O?> command, I? args) {
        var user = UnitOfWork.User;
        var username = user.Username ?? "Anonymous";
        Log.LogInformation($"{username} is executing Command {command.Name}");

        Authenticate(command, user);
        await command.Validate(user, args);
        var result = await command.Run(args);
        await UnitOfWork.Commit(); // saves db changes
        
        var events = UnitOfWork.Events;
        if (events.Count > 0) {
            DispatchEvents(events);
        }
        
        return result;
    }

    // private async Task<object?> RunCommandWithLogging(CqsCommand command, object? args, bool includeArguments) {
    //     var commandName = command.Name;
    //     var actionLog = new ActionLog(
    //         name: commandName,
    //         type: ActionType.Command,
    //         arguments: includeArguments && args != null ? IJsonParser.Default.WriteJson(args) : null
    //     );
    //
    //     try {
    //         var result = await RunCommand(command, args, actionLog.Id);
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

    public async Task<O?> RunQuery<I, O>(Query<I?, O?> query, I? args) {
        var user = UnitOfWork.User;
        var username = user.Id?.Value.ToString("N") ?? "Anonymous";
        Log.LogInformation($"{username} is executing Query {query.Name}");

        Authenticate(query, user);
        await query.Validate(user, args);
        return await query.Run(args);
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