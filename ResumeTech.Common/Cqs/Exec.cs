using NLog;
using ResumeTech.Common.Cqs.Commands;
using ResumeTech.Common.Cqs.Queries;
using ResumeTech.Common.Events;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Service;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs; 

/**
 * Executes Commands and Queries with common operations, such as logging and event dispatching.
 */
public class Exec {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private IScopeProvider ScopeProvider { get; }
    private IUnitOfWork UnitOfWork { get; }
    private IEventDispatcher EventDispatcher { get; }
    // private IActionLogRepository ActionLogRepository { get; }

    public Exec(IScopeProvider scopeProvider, IUnitOfWork unitOfWork, IEventDispatcher eventDispatcher) {
        ScopeProvider = scopeProvider;
        UnitOfWork = unitOfWork;
        EventDispatcher = eventDispatcher;
    }

    /**
     * Executes a CQS Command with arguments and returns its result
     */
    public Task<O> Command<I, O>(CqsCommand<I, O> command, I args) {
        return ExecuteCommand(command, args).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Command returned null");
        });
    }
    
    /**
     * Executes a CQS Command and returns its result
     */
    public Task<O> Command<O>(CqsCommand<O> command) {
        return ExecuteCommand(command, null).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Command returned null");
        });
    }
    
    /**
     * Executes a Pure CQS Command with arguments and returns an empty task
     */
    public Task Command<I>(PureCqsCommand<I> command, I args) {
        return ExecuteCommand(command, args);
    }
    
    /**
     * Executes a Pure CQS Command and returns an empty task
     */
    public Task Command(PureCqsCommand command) {
        return ExecuteCommand(command, null);
    }
    
    private Task<object?> ExecuteCommand(CqsCommand command, object? args) {
        var logPolicy = command.LogPolicy;
        Log.Info($"Executing Command {command.Name} with Log Policy {logPolicy}");
        return ExecuteCommandWithoutLogging(command, args);
    }

    private async Task<object?> ExecuteCommandWithoutLogging(CqsCommand command, object? args) {
        var result = await command.Execute(args);
        var events = UnitOfWork.Commit();
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
    //     catch (NummiException e) {
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

    public Task<O> Query<I, O>(CqsQuery<I, O> query, I args) {
        return RunQuery(query, args).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Query returned null");
        });

    }
    
    public Task<O> Query<O>(CqsQuery<O> query) {
        return RunQuery(query, null).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (O)t.Result.OrElseThrow("Query returned null");
        });
    }

    private Task<object> RunQuery(CqsQuery query, object? args) {
        Log.Info($"Executing Query {query.Name}");
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
    //     catch (NummiException e) {
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