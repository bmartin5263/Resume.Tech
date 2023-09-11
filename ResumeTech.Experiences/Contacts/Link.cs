using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Contacts; 

public sealed record Link {
    public const int MaxFieldLength = 80;

    public string WebsiteName { get; }
    public Uri Uri { get; }

    private Link() {
        WebsiteName = null!;
        Uri = null!;
    }

    public Link(string WebsiteName, Uri Uri) {
        this.WebsiteName = WebsiteName.AssertMaxTrimmedLength(MaxFieldLength, "Website Name");
        this.Uri = Uri;
    }

    public void Deconstruct(out string WebsiteName, out Uri Uri) {
        WebsiteName = this.WebsiteName;
        Uri = this.Uri;
    }
}