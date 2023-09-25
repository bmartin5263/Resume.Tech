using System.ComponentModel.DataAnnotations;
using ResumeTech.Experiences.Common;

namespace ResumeTech.Experiences.Jobs.Dto;

public sealed record AddPositionRequest(
    string Title,
    DateOnlyRange Dates,
    IList<string>? BulletPoints
) {
    public Position ToEntity() => new(
        Title: Title,
        Dates: Dates,
        BulletPoints: BulletPoints?.Select(b => new BulletPoint(b))
    );
}