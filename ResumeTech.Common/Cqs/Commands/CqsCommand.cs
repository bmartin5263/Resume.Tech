using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs.Commands;

public abstract class CqsCommand {
    public abstract string Name { get; }
    public virtual LogPolicy LogPolicy => LogPolicy.IncludeArguments;
    
    public abstract Task<object?> Execute(object? args);

    public virtual Task Rollback(object? args) {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class CqsCommand<I, O> : CqsCommand {
    public override Task<object?> Execute(object? args) {
        return Execute((I)args.OrElseThrow("Unexpected null command arguments")).ContinueWith(t => {
            if (t.IsFaulted) {
                throw t.Exception!.InnerException!;
            }
            return (object?) t.Result;
        });
    }

    public abstract Task<O> Execute(I args);

    public override Task Rollback(object? args) {
        return Rollback((I)args.OrElseThrow("Unexpected null command arguments"));
    }

    public virtual Task Rollback(I args) {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class CqsCommand<O> : CqsCommand {
    public override Task<object?> Execute(object? args) {
        return Execute().ContinueWith(t => (object?)t.Result);
    }

    public abstract Task<O> Execute();

    public override Task Rollback(object? args) {
        return Rollback();
    }

    public virtual Task Rollback() {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class PureCqsCommand<I> : CqsCommand {
    public override Task<object?> Execute(object? args) {
        return Execute((I)args.OrElseThrow("Unexpected null command arguments")).ContinueWith(_ => (object?) null);
    }

    public abstract Task Execute(I args);

    public override Task Rollback(object? args) {
        return Rollback();
    }

    public virtual Task Rollback() {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class PureCqsCommand : CqsCommand {
    public override Task<object?> Execute(object? args) {
        return Execute().ContinueWith(_ => (object?) null);
    }

    public abstract Task Execute();

    public override Task Rollback(object? args) {
        return Rollback();
    }

    public virtual Task Rollback() {
        throw new AppException("Command does not support rollbacks");
    }
}