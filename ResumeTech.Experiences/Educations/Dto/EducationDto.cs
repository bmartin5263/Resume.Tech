using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Profiles;

namespace ResumeTech.Experiences.Educations.Dto;

public record EducationDto(
    EducationId? Id,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    UserId? OwnerId
);

public static class EducationDtoUtil {
    public static EducationDto ToDto(this Education education) {
        return new EducationDto(
            Id: education.Id,
            CreatedAt: education.CreatedAt,
            UpdatedAt: education.UpdatedAt,
            DeletedAt: education.DeletedAt,
            OwnerId: education.OwnerId
        );
    }
}