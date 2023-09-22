using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs;

public abstract class PureCqsCommand<I> : CqsCommand {
    public override Task<object?> Execute(object? args) {
        var a = (I) args.OrElseThrow("Missing arguments");
        return Execute(a).InjectNull();
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
        return Execute().InjectNull();
    }

    public abstract Task Execute();

    public override Task Rollback(object? args) {
        return Rollback();
    }

    public virtual Task Rollback() {
        throw new AppException("Command does not support rollbacks");
    }
}