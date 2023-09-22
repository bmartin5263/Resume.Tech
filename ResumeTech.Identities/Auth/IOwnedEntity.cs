using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Auth; 

public interface IOwnedEntity {
    UserId OwnerId { get; }
}