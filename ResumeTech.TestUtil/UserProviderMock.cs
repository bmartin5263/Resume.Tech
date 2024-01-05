using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.TestUtil; 

public class UserProviderMock : IUserProvider {
    public UserDetails CurrentUser { get; private set; }
    public UserId CurrentUserId => CurrentUser.Id!.Value.OrElseThrow("Current user is not set");

    public UserProviderMock(UserDetails currentUser) {
        CurrentUser = currentUser;
    }

    public void Login(UserDetails userDetails) {
        if (CurrentUser.Id != null) {
            throw new InvalidOperationException("Cannot set UserId twice");
        }
        CurrentUser = userDetails;
    }
}