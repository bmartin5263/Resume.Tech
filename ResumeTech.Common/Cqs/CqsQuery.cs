using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs; 

public abstract class CqsQuery : IAction<object?, object?> {
    public abstract string Name { get; }
    public virtual bool AllowAnonymous => true;
    public virtual IReadOnlySet<RoleName> Roles => ReadOnly.Set<RoleName>();
    
    public abstract Task<object?> Execute(object? args);
}

public abstract class CqsQuery<I, O> : CqsQuery, IAction<I, O> {
    public override Task<object?> Execute(object? args) {
        I a = (I) args.OrElseThrow("Missing arguments");
        return Execute(a).CastReverse();
    }

    public abstract Task<O> Execute(I args);
}

public abstract class CqsQuery<O> : CqsQuery, IAction<object?, O> {
    public override Task<object?> Execute(object? args) {
        return Execute().CastReverse();
    }

    public abstract Task<O> Execute();
}