using ResumeTech.Common.Auth;

namespace ResumeTech.Identities.Auth; 

public interface IMultiOwnedEntity {
    ISet<UserId> OwnerIds { get; set; }
}