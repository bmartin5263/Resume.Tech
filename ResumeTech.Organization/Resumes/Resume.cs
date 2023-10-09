using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Educations;

namespace ResumeTech.Organization.Resumes;

public class Resume : IEntity<ResumeId>, IAuditedEntity, ISoftDeletable {
    /// <summary>
    /// Custom name for User to identify this Resume
    /// </summary>
    public string Nickname { get; set; }
    
    /// <summary>
    /// Contact Info linked to this resume. Will be used for populating contact information such as name, address
    /// email, etc
    /// </summary>
    public ContactInfoId ContactInfoId { get; set; }
    
    /// <summary>
    /// Professional summary, usually displayed at the top of the Resume
    /// </summary>
    public string? Summary { get; set; }
    
    /// <summary>
    /// Jobs listed on this Resume
    /// </summary>
    public IList<ResumeJob> Jobs { get; set; }

    /// <summary>
    /// Educations listed on this Resume
    /// </summary>
    public IList<ResumeEducation> Educations { get; set; }

    // Common Entity Properties
    public ResumeId Id { get; private set; } = ResumeId.Generate();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Default Constructor Needed for Persistence
    private Resume() {
        Nickname = null!;
        Jobs = null!;
        Educations = null!;
    }

    public Resume(string Nickname) {
        this.Nickname = Nickname;
        this.Jobs = new List<ResumeJob>();
        this.Educations = new List<ResumeEducation>();
    }
}