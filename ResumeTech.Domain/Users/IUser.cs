using System.Collections;
using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;
using ResumeTech.Domain.Experience;
using ResumeTech.Domain.Resumes;

namespace ResumeTech.Domain.Users;

public interface IUser : IEntity {
    public UserId Id { get; }
    public string Username { get; }
    public IList<ContactDetails> ContactDetails { get; }
    public IList<JobId> Jobs { get; }
    public IList<ResumeId> Resumes { get; }
}