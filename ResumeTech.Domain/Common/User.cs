using System.Collections.Immutable;
using ResumeTech.Domain.Experience;
using ResumeTech.Domain.Resumes;
using ResumeTech.Domain.Util;

namespace ResumeTech.Domain.Common; 

public class User : IUser {
    private IList<ContactDetails>? _contactDetails;
    public IReadOnlyList<ContactDetails> ContactDetails => _contactDetails.ToReadOnly();
    
    private IList<Job>? _jobs;
    public IReadOnlyList<Job> Jobs => _jobs.ToReadOnly();
    
    private IList<Skill>? _skills;
    public IReadOnlyList<Skill> Skills => _skills.ToReadOnly();

    private IList<Resume>? _resumes;
    public IReadOnlyList<Resume> Resumes => _resumes.ToReadOnly();
    
    // Common Entity Properties
    public UserId Id { get; private set; } = UserId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public User() {
    }

    public void AddResume(Resume resume) {
        CheckCanAddResume(resume);
        _resumes ??= new List<Resume>();
        _resumes.Add(resume);
    }

    private void CheckCanAddResume(Resume resume) {
        if (_resumes == null) {
            return;
        }
        var nicknameAlreadyExists = _resumes.Any(r => r.Nickname == resume.Nickname);
        if (nicknameAlreadyExists) {
            throw new ArgumentException($"Resume with Nickname '{resume.Nickname}' already exists");
        }
    }

    public void AddSkill(Skill skill) {
        _skills ??= new List<Skill>();
        _skills.Add(skill);
    }
    
    public void AddJob(Job experience) {
        _jobs ??= new List<Job>();
        _jobs.Add(experience);
    }
    
    public void AddContactDetails(ContactDetails contactDetails) {
        _contactDetails ??= new List<ContactDetails>();
        _contactDetails.Add(contactDetails);
    }
}