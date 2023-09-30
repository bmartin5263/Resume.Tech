using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Contacts.Dto;
using ResumeTech.Experiences.Educations;
using ResumeTech.Experiences.Educations.Dto;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Dto;
using ResumeTech.Experiences.Projects;
using ResumeTech.Experiences.Projects.Dto;

namespace ResumeTech.Experiences.Profiles.Dto; 

public record ProfileDto(
    ProfileId? Id = null,
    DateTimeOffset? CreatedAt = null,
    DateTimeOffset? UpdatedAt = null,
    DateTimeOffset? DeletedAt = null,
    UserId? OwnerId = null,
    IEnumerable<ContactInfoDto>? ContactInfos = null,
    IEnumerable<JobDto>? Jobs = null,
    IEnumerable<ProjectDto>? Projects = null,
    IEnumerable<EducationDto>? Educations = null
);

public static class ProfileDtoUtil {
    public static ProfileDto ToDto(this Profile profile) {
        return new ProfileDto(
            Id: profile.Id,
            CreatedAt: profile.CreatedAt,
            UpdatedAt: profile.UpdatedAt,
            DeletedAt: profile.DeletedAt,
            OwnerId: profile.OwnerId,
            ContactInfos: profile.ContactInfos?.Select(c => c.ToDto()),
            Jobs: profile.Jobs?.Select(j => j.ToDto()),
            // Projects: profile.Projects?.Select(p => p.ToDto()),
            Educations: profile.Educations?.Select(p => p.ToDto())
        );
    }
}