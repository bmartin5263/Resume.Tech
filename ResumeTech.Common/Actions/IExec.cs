namespace ResumeTech.Common.Actions;

public interface IExec {
    Task<O> Command<I, O>(Command<I, O> command, I args) where O : notnull;
    Task<O> Command<O>(Command<O> command) where O : notnull;
    Task Command<I>(PureCommand<I> command, I args);
    Task Command(PureCommand command);
    Task<O> Query<I, O>(Query<I, O> query, I args);
    Task<O> Query<O>(Query<O> query);
}