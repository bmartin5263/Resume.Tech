using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Profiles;

namespace ResumeTech.Experiences.Jobs.Dto; 

public sealed record JobDto(
    JobId Id = default,
    DateTimeOffset CreatedAt = default,
    DateTimeOffset? UpdatedAt = default,
    DateTimeOffset? DeletedAt = default,
    ProfileId? OwnerId = default,
    string? CompanyName = default,
    Location? Location = default,
    IEnumerable<PositionDto>? Positions = default
);

public static class JobDtoUtil {
    public static JobDto ToDto(this Job job) {
        return new JobDto(
            Id: job.Id,
            CreatedAt: job.CreatedAt,
            UpdatedAt: job.UpdatedAt,
            DeletedAt: job.DeletedAt,
            OwnerId: job.OwnerId,
            CompanyName: job.CompanyName,
            Location: job.Location,
            Positions: job.Positions.Select(p => p.ToDto())
        );
    }
}