namespace ResumeTech.Domain.Site; 

public class IntroPage : IPage {
    // Common Entity Properties
    public IntroPageId Id { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public IntroPage() {
        Id = IntroPageId.Generate();
    }
    
    public PageType Type => PageType.Intro;
}