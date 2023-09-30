using ResumeTech.Common.Auth;

namespace ResumeTech.Experiences.Projects.Dto; 

public record ProjectDto(
    IProjectId? Id,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    UserId? OwnerId
);

public static class ProjectDtoUtil {
    public static ProjectDto ToDto(this IProject project) {
        return new ProjectDto(
            Id: project.ProjectId,
            CreatedAt: project.CreatedAt,
            UpdatedAt: project.UpdatedAt,
            DeletedAt: project.DeletedAt,
            OwnerId: project.OwnerId
        );
    }
}