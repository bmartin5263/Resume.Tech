using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Projects; 

public abstract class IProject : IAuditedEntity, ISoftDeletable {
    public abstract UserId OwnerId { get; protected set; }
    public abstract IProjectId ProjectId { get; }
    public abstract string Name { get; set; }
    public abstract DateOnlyRange Dates { get; set; }
    public abstract DateTimeOffset CreatedAt { get; set; }
    public abstract DateTimeOffset? UpdatedAt { get; set; }
    public abstract DateTimeOffset? DeletedAt { get; set; }
}