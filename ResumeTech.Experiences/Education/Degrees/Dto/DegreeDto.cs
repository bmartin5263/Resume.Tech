using ResumeTech.Common.Auth;

namespace ResumeTech.Experiences.Education.Degrees.Dto;

public record DegreeDto(
    DegreeId? Id,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    UserId? OwnerId
);

public static class EducationDtoUtil {
    public static DegreeDto ToDto(this Degree degree) {
        return new DegreeDto(
            Id: degree.Id,
            CreatedAt: degree.CreatedAt,
            UpdatedAt: degree.UpdatedAt,
            DeletedAt: degree.DeletedAt,
            OwnerId: degree.OwnerId
        );
    }
}