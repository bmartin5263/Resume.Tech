using System.Collections.Immutable;
using ResumeTech.Common;
using ResumeTech.Common.Domain;
using ResumeTech.Domain.Common;

namespace ResumeTech.Domain.Resumes;

public class Resume : IEntity {
    
    public string Nickname { get; set; }
    
    public ContactDetails? ContactDetails { get; set; }

    private IList<IResumeSection>? _sections;
    private IReadOnlyList<IResumeSection>? Sections => _sections?.AsReadOnly();

    // Common Entity Properties
    public ResumeId Id { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    private Resume() {
        Nickname = null!;
        ContactDetails = null!;
    }

    public Resume(string nickname, PersonName ownerName) {
        Id = ResumeId.Generate();
        Nickname = nickname;
        ContactDetails = new ContactDetails(ownerName);
        _sections = new List<IResumeSection>();
    }

    public void AppendSection(IResumeSection section) {
        _sections ??= new List<IResumeSection>();
        
        var type = section.Type;
        var currentSectionCount = _sections.Count(v => v.Type == type);
        var maxSectionCount = MaxSectionsByType[type];

        if (currentSectionCount >= maxSectionCount) {
            throw new ArgumentException("Reached max number of sections for a given type");
        }
        
        _sections.Add(section);
    }
    
    // TODO - pay money to unlock more sections?
    // TODO - throw error on startup when type is missing from dictionary
    private static ImmutableDictionary<ResumeSectionType, int> MaxSectionsByType = 
        new Dictionary<ResumeSectionType, int> {
            { ResumeSectionType.Headline, 1 },
            { ResumeSectionType.Summary, 1 },
            { ResumeSectionType.Education, 2 },
            { ResumeSectionType.Experience, 3 },
            { ResumeSectionType.Projects, 3 },
            { ResumeSectionType.Skills, 1 },
            { ResumeSectionType.Links, 1 }
        }.ToImmutableDictionary();
}