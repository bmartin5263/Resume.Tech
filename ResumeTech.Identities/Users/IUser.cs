using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Users;

public interface IUser : IEntity<UserId>, IAuditedEntity, ISoftDeletable {
    public string UserName { get; }
    public EmailAddress EmailAddress { get; }
}