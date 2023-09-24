using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Actions;

public abstract class Command<I, O> : Action<I, O> {
}

public abstract class Command<O> : Command<object?, O>  {
    public sealed override Task<O> Run(object? args) {
        return Run();
    }

    public abstract Task<O> Run();
    
    public sealed override Task Validate(UserDetails user, object? args) {
        return Validate(user);
    }

    public virtual Task Validate(UserDetails user) {
        return Task.CompletedTask;
    }
}