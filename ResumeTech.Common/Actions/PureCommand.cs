using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Actions;

public abstract class PureCommand<I> : Command<I, object> {
    public override async Task<object> Run(I args) {
        await RunWithoutResult(args);
        return new object();
    }

    public abstract Task RunWithoutResult(I args);
}

public abstract class PureCommand : PureCommand<object>  {
    public sealed override async Task<object> RunWithoutResult(object? args) {
        await RunWithoutResult();
        return new object();
    }

    public abstract Task RunWithoutResult();
    
    public sealed override Task Validate(UserDetails user, object? args) {
        return Validate(user);
    }

    public virtual Task Validate(UserDetails user) {
        return Task.CompletedTask;
    }
}