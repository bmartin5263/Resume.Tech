using ResumeTech.Common.Utility;

namespace ResumeTech.Identities.Domain; 

public class UserIdProvider {
    private UserId? userId;

    public void Set(UserId id) {
        if (userId != null) {
            throw new InvalidOperationException("Cannot set UserId twice");
        }
        userId = id;
    }

    public UserId Get() {
        if (userId == null) {
            throw new InvalidOperationException("UserId has not been set");
        }
        return userId.Value;
    }
}