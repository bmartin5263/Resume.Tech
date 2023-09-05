namespace ResumeTech.Domain.Site; 

public class PortfolioPage : IPage {
    // Common Entity Properties
    public PortfolioPageId Id { get; private set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public PortfolioPage() {
        Id = PortfolioPageId.Generate();
    }
    
    public PageType Type => PageType.Portfolio;
}