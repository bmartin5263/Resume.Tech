using ResumeTech.Domain.Experience;
using ResumeTech.Domain.Resumes;

namespace ResumeTech.Domain.Common;

public interface IUser : IAudited {
    public UserId Id { get; }
    public IReadOnlyList<ContactDetails> ContactDetails { get; }
    public IReadOnlyList<Job> Jobs { get; }
    public IReadOnlyList<Resume> Resumes { get; }
}