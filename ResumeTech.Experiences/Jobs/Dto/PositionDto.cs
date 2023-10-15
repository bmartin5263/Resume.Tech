using ResumeTech.Common.Domain;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Jobs.Dto;

public sealed record PositionDto(
    PositionId Id = default,
    DateTimeOffset? CreatedAt = default,
    DateTimeOffset? UpdatedAt = default,
    string? Title = default,
    DateOnlyRange? Dates = default,
    RichText? Description = default,
    IList<BulletPoint>? BulletPoints = default
);

public static class PositionDtoUtil {
    public static PositionDto ToDto(this Position position) {
        return new PositionDto(
            Id: position.Id,
            CreatedAt: position.CreatedAt,
            UpdatedAt: position.UpdatedAt,
            Title: position.Title,
            Dates: position.Dates,
            Description: position.Description,
            BulletPoints: position.BulletPoints
        );
    }
}