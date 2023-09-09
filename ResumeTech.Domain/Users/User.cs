using ResumeTech.Domain.Common;
using ResumeTech.Domain.Experience;
using ResumeTech.Domain.Portfolios;
using ResumeTech.Domain.Resumes;
using ResumeTech.Domain.Util;

namespace ResumeTech.Domain.Users; 

public class User : IUser {
    
    public string Username { get; private set; }
    public IList<ExternalLink> ExternalLinks { get; private set; } = new List<ExternalLink>();
    public IList<Skill> Skills { get; private set; } = new List<Skill>();
    public IList<ContactDetails> ContactDetails { get; private set; } = new List<ContactDetails>();

    // Aggregate Relationships
    public IList<JobId> Jobs { get; private set; } = new List<JobId>();
    public IList<ResumeId> Resumes { get; private set; } = new List<ResumeId>();
    public IList<PortfolioId> Portfolios { get; private set; } = new List<PortfolioId>();

    // Common Entity Properties
    public UserId Id { get; private set; } = UserId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    private User() {
        Username = null!;
    }
    
    public User(string Username) {
        this.Username = Username;
    }
}