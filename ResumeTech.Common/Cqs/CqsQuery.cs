using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs; 

public abstract class CqsQuery {
    public abstract string Name { get; }
    public virtual bool AllowAnonymous => false;
    public virtual IReadOnlySet<RoleName> Roles => ReadOnly.Set<RoleName>();
    
    public abstract Task<object> Execute(object? args);
}

public abstract class CqsQuery<I, O> : CqsQuery {
    public override async Task<object> Execute(object? args) {
        var result = await Execute((I)args.OrElseThrow("Unexpected null query arguments"));
        return result.OrElseThrow("Query returned null")!;
    }

    public abstract Task<O> Execute(I args);
}

public abstract class CqsQuery<O> : CqsQuery {
    public override async Task<object> Execute(object? args) {
        var result = await Execute();
        return result.OrElseThrow("Query returned null")!;
    }

    public abstract Task<O> Execute();
}