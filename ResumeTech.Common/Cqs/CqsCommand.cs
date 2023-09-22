using ResumeTech.Common.Auth;
using ResumeTech.Common.Exceptions;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs;

public abstract class CqsCommand : IAction<object?, object?> {
    public abstract string Name { get; }
    public virtual LogPolicy LogPolicy => LogPolicy.IncludeArguments;
    public virtual bool AllowAnonymous => true;
    public virtual IReadOnlySet<RoleName> RequiresAnyRole => ReadOnly.Set<RoleName>();
    public virtual IReadOnlySet<RoleName> RequiresAllRoles => ReadOnly.Set<RoleName>();

    public abstract Task<object?> Execute(object? args);

    public virtual Task Rollback(object? args) {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class CqsCommand<I, O> : CqsCommand, IAction<I, O> {
    public override Task<object?> Execute(object? args) {
        var a = (I) args.OrElseThrow("Missing arguments");
        return Execute(a).CastReverse();
    }

    public abstract Task<O> Execute(I args);

    public override Task Rollback(object? args) {
        return Rollback((I)args.OrElseThrow("Unexpected null command arguments"));
    }

    public virtual Task Rollback(I args) {
        throw new AppException("Command does not support rollbacks");
    }
}

public abstract class CqsCommand<O> : CqsCommand, IAction<object?, O> {
    public override Task<object?> Execute(object? args) {
        return Execute().CastReverse();
    }

    public abstract Task<O> Execute();

    public override Task Rollback(object? args) {
        return Rollback();
    }

    public virtual Task Rollback() {
        throw new AppException("Command does not support rollbacks");
    }
}