using ResumeTech.Common.Auth;
using ResumeTech.Common.Domain;

namespace ResumeTech.Identities.Users;

public sealed record UserDto(
    UserId Id,
    string Username,
    EmailAddress Email
);

public static class UserDtoUtils {
    public static UserDto ToDto(this IUser user) {
        return new UserDto(
            Id: user.Id,
            Username: user.UserName,
            Email: user.EmailAddress
        );
    }
}