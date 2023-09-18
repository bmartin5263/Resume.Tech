using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth; 

public interface IOwnedEntity {
    UserId OwnerId { get; }
}