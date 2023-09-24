namespace ResumeTech.Common.Actions;

public abstract class Query<I, O> : Action<I, O> {
}

public abstract class Query<O> : Query<object?, O> {
    public override Task<O> Run(object? args) {
        return Run();
    }

    public abstract Task<O> Run();
}