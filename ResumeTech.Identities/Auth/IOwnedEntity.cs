using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Auth; 

public interface IOwnedEntity<out T> {
    T OwnerId { get; }
}