namespace ResumeTech.Common.Events;

public abstract class DomainEventHandler {
    public abstract string Name { get; }
    public abstract Task OnEvent(IDomainEvent domainEvent);
}

public abstract class DomainEventHandler<E> : DomainEventHandler where E : IDomainEvent  {
    public override Task OnEvent(IDomainEvent domainEvent) {
        return OnEvent((E) domainEvent);
    }

    public abstract Task OnEvent(E domainEvent);
}
