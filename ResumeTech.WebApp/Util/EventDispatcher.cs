using ResumeTech.Common.Events;

namespace ResumeTech.Application.Util; 

public class EventDispatcher : IEventDispatcher {
    public void Initialize() {
        
    }

    public Task Dispatch(IEnumerable<IDomainEvent> events) {
        return Task.CompletedTask;
    }
}