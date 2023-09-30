using ResumeTech.Common.Auth;

namespace ResumeTech.Experiences.Contacts.Dto; 

public record ContactInfoDto(
    ContactInfoId? Id = null,
    DateTimeOffset? CreatedAt = null,
    DateTimeOffset? UpdatedAt = null,
    DateTimeOffset? DeletedAt = null
);

public static class ContactInfoDtoUtil {
    public static ContactInfoDto ToDto(this ContactInfo contactInfo) {
        return new ContactInfoDto(
            Id: contactInfo.Id,
            CreatedAt: contactInfo.CreatedAt,
            UpdatedAt: contactInfo.UpdatedAt,
            DeletedAt: contactInfo.DeletedAt
        );
    }
}