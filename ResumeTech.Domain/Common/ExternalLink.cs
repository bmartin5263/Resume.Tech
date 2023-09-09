namespace ResumeTech.Domain.Common;

public record ExternalLink(Uri Value, string SiteName, string? Description = null) {
    private ExternalLink() : this(null!, null!) {}
}