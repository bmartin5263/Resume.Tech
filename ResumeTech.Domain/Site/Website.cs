using ResumeTech.Domain.Common;
using ResumeTech.Domain.Util;

namespace ResumeTech.Domain.Site; 

public class Website : IAudited {

    private IList<IPage>? _pages;
    public IReadOnlyList<IPage> Pages => _pages.ToReadOnly();

    // Common Entity Properties
    public WebsiteId Id { get; private set; } = WebsiteId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public void AddPage(IPage page) {
        _pages ??= new List<IPage>();
        _pages.Add(page);
    }
}