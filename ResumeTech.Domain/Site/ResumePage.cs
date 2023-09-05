namespace ResumeTech.Domain.Site; 

public class ResumePage : IPage {
    // Common Entity Properties
    public ResumePageId Id { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public ResumePage() {
        Id = ResumePageId.Generate();
    }

    public PageType Type => PageType.Resume;
}