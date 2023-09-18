using NLog;
using ResumeTech.Common.Events;
using ResumeTech.Common.Service;
using ResumeTech.Common.Utility;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Users;

namespace ResumeTech.Cqs; 

/**
 * Executes Commands and Queries with common operations, such as logging and event dispatching.
 */
public class Exec {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IUnitOfWork UnitOfWork { get; }
    private IEventDispatcher EventDispatcher { get; }
    private IdentityProvider IdentityProvider { get; }
    // private IActionLogRepository ActionLogRepository { get; }

    public Exec(IUnitOfWork unitOfWork, IEventDispatcher eventDispatcher, IdentityProvider identityProvider) {
        UnitOfWork = unitOfWork;
        EventDispatcher = eventDispatcher;
        IdentityProvider = identityProvider;
    }

    /**
     * Executes a CQS Command with arguments and returns its result
     */
    public async Task<O> Command<I, O>(CqsCommand<I, O> command, I args, UserDetails userDetails) {
        var result = await ExecuteCommand(command, args, userDetails);
        return (O) result.OrElseThrow("Command returned null");
    }
    
    /**
     * Executes a CQS Command and returns its result
     */
    public async Task<O> Command<O>(CqsCommand<O> command, UserDetails userDetails) {
        var result = await ExecuteCommand(command, null, userDetails);
        return (O) result.OrElseThrow("Command returned null");
    }
    
    /**
     * Executes a Pure CQS Command with arguments and returns an empty task
     */
    public Task Command<I>(PureCqsCommand<I> command, I args, UserDetails userDetails) {
        return ExecuteCommand(command, args, userDetails);
    }
    
    /**
     * Executes a Pure CQS Command and returns an empty task
     */
    public Task Command(PureCqsCommand command, UserDetails userDetails) {
        return ExecuteCommand(command, null, userDetails);
    }
    
    private Task<object?> ExecuteCommand(CqsCommand command, object? args, UserDetails userDetails) {
        var logPolicy = command.LogPolicy;
        Log.Info($"Executing Command {command.Name} with Log Policy {logPolicy}");
        return ExecuteCommandWithoutLogging(command, args, userDetails);
    }

    private async Task<object?> ExecuteCommandWithoutLogging(CqsCommand command, object? args, UserDetails userDetails) {
        IdentityProvider.Set(userDetails);
        var anyRoles = command.AnyRole;
        if (anyRoles.Count > 0 && anyRoles.Overlaps(userDetails.Roles)) {
            throw new AccessDeniedException(DeveloperMessage: $"Required Any Role: {anyRoles}");
        }
        
        var result = await command.Execute(args);
        var events = await UnitOfWork.Commit();
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

    public Task<O> Query<I, O>(CqsQuery<I, O> query, I args, UserDetails userDetails) {
        return RunQuery(query, args, userDetails).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Query returned null");
        });

    }
    
    public Task<O> Query<O>(CqsQuery<O> query, UserDetails userDetails) {
        return RunQuery(query, null, userDetails).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Query returned null");
        });
    }

    private Task<object> RunQuery(CqsQuery query, object? args, UserDetails userDetails) {
        Log.Info($"Executing Query {query.Name}");
        
        IdentityProvider.Set(userDetails);
        var anyRoles = query.AnyRole;
        if (anyRoles.Count > 0 && anyRoles.Overlaps(userDetails.Roles)) {
            throw new AccessDeniedException(DeveloperMessage: $"Required Any Role: {anyRoles}");
        }

        return query.Execute(args);
    }

    public Task EventHandler(DomainEventHandler handler, IDomainEvent domainEvent) {
        Log.Info($"Executing Event Handler {handler.Name} for Event {domainEvent.Name}");
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
                Log.Error($"Failed to dispatch domain events: {e}");
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
//     public static Task<O> Execute<I, O>(this IAppServiceProvider serviceProvider, CqsCommand<I, O> command, I args, UserDetails userDetails) {
//         return serviceProvider.GetService<Exec>().Command(command, args, userDetails);
//     }
//     
//     public static Task<O> Execute2<I, O>(this IAppServiceProvider serviceProvider, I args, UserDetails userDetails) 
//     {
//         var command = serviceProvider.GetService<CqsCommand<I, O>>();
//         return serviceProvider.GetService<Exec>().Command(command, args, userDetails);
//     }
//     
//     public static Task<O> Execute<O>(this IAppServiceProvider serviceProvider, CqsCommand<O> command, UserDetails userDetails) {
//         return serviceProvider.GetService<Exec>().Command(command, userDetails);
//     }
//     
//     public static Task Execute<I>(this IAppServiceProvider serviceProvider, PureCqsCommand<I> command, I args, UserDetails userDetails) {
//         return serviceProvider.GetService<Exec>().Command(command, args, userDetails);
//     }
//
//     public static Task Execute(this IAppServiceProvider serviceProvider, PureCqsCommand command, UserDetails userDetails) {
//         return serviceProvider.GetService<Exec>().Command(command, userDetails);
//     }
//     
// }