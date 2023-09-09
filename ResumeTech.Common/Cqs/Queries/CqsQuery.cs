using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs.Queries; 

public abstract class CqsQuery {
    public abstract string Name { get; }
    public abstract Task<object> Execute(object? args);
}

public abstract class CqsQuery<I, O> : CqsQuery {
    public override Task<object> Execute(object? args) {
        return Execute((I)args.OrElseThrow("Unexpected null query arguments"))
            .ContinueWith(t => (object)t.Result.OrElseThrow("Query returned null")!);
    }

    public abstract Task<O> Execute(I args);
}

public abstract class CqsQuery<O> : CqsQuery {
    public override Task<object> Execute(object? args) {
        return Execute().ContinueWith(t => (object)t.Result.OrElseThrow("Query returned null")!);
    }

    public abstract Task<O> Execute();
}