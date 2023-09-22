namespace ResumeTech.Common.Events; 

public interface IEventPublisher {
    public IEnumerable<IDomainEvent> DomainEvents { get; }
}