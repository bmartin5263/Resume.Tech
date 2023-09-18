using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth; 

public interface IMultiOwnedEntity {
    ISet<UserId> OwnerIds { get; set; }
}