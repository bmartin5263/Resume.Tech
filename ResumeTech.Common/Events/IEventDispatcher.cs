namespace ResumeTech.Common.Events;

public interface IEventDispatcher {
    public void Initialize();
    public Task Dispatch(IEnumerable<IDomainEvent> events);
}