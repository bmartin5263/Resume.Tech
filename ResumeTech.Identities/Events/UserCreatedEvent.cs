using ResumeTech.Common.Domain;
using ResumeTech.Common.Events;

namespace ResumeTech.Identities.Events; 

public class UserCreatedEvent : IDomainEvent {
    public string Name => "UserCreated";

    public EmailAddress EmailAddress { get; }
    public bool IsEmailConfirmed { get; }

    public UserCreatedEvent(EmailAddress emailAddress, bool isEmailConfirmed) {
        EmailAddress = emailAddress;
        IsEmailConfirmed = isEmailConfirmed;
    }
}