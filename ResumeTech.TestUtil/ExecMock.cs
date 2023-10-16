using ResumeTech.Common.Actions;

namespace ResumeTech.TestUtil; 

public class ExecMock : IExec {
    
    public Task<O> Command<I, O>(Command<I, O> command, I args) where O : notnull {
        return command.Run(args);
    }

    public Task<O> Command<O>(Command<O> command) where O : notnull {
        return command.Run();
    }

    public Task Command<I>(PureCommand<I> command, I args) {
        return command.RunWithoutResult(args);
    }

    public Task Command(PureCommand command) {
        return command.RunWithoutResult();
    }

    public Task<O> Query<I, O>(Query<I, O> query, I args) {
        return query.Run(args);
    }

    public Task<O> Query<O>(Query<O> query) {
        return query.Run();
    }
    
}