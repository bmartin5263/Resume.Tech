using ResumeTech.Identities.Domain;

namespace ResumeTech.Experiences.Jobs; 

public sealed record JobDto(
    JobId Id = default,
    UserId OwnerId = default,
    DateTimeOffset CreatedAt = default,
    DateTimeOffset? UpdatedAt = default,
    DateTimeOffset? DeletedAt = default,
    string? Name = default
);

public static class JobDtoUtil {
    public static JobDto ToDto(this Job job) {
        return new JobDto(
            Id: job.Id,
            OwnerId: job.OwnerId,
            CreatedAt: job.CreatedAt,
            UpdatedAt: job.UpdatedAt,
            DeletedAt: job.DeletedAt,
            Name: job.CompanyName
        );
    }
}