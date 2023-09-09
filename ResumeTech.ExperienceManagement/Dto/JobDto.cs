using ResumeTech.ExperienceManagement.Domain;

namespace ResumeTech.ExperienceManagement.Dto; 

public record JobDto(
    JobId Id = default,
    DateTimeOffset CreatedAt = default,
    DateTimeOffset? UpdatedAt = default,
    DateTimeOffset? DeletedAt = default,
    string? Name = default
);

public static class JobDtoUtil {
    public static JobDto ToDto(this Job job) {
        return new JobDto(
            Id: job.Id,
            CreatedAt: job.CreatedAt,
            UpdatedAt: job.UpdatedAt,
            DeletedAt: job.DeletedAt,
            Name: job.CompanyName
        );
    }
}