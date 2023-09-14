namespace ResumeTech.Identities.Domain; 

public interface IMultiOwnedEntity {
    ISet<UserId> OwnerIds { get; set; }
}