namespace ResumeTech.Identities.Domain; 

public interface IOwnedEntity {
    UserId OwnerId { get; }
}