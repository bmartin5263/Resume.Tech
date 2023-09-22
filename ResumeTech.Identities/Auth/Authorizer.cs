using ResumeTech.Common.Auth;
using ResumeTech.Identities.Auth.Filters;
using ResumeTech.Identities.Users;

namespace ResumeTech.Identities.Auth; 

public class Authorizer<TEntity> where TEntity : class {
    private IList<IAccessFilter<TEntity>> Filters { get; }
    private IUserDetailsProvider UserDetailsProvider { get; }
    public UserDetails CurrentUser => UserDetailsProvider.CurrentUser;

    public Authorizer(IList<IAccessFilter<TEntity>> Filters, IUserDetailsProvider UserDetailsProvider) {
        this.Filters = Filters;
        this.UserDetailsProvider = UserDetailsProvider;
    }

    public async Task<TEntity> AssertCanRead(TEntity entity) {
        if (!await CanRead(entity)) {
            throw new AccessDeniedException();
        }
        return entity;
    }
    

    public async Task<bool> CanRead(TEntity entity) {
        var userId = UserDetailsProvider.CurrentUser.Id;
        if (userId == null) {
            throw new AccessDeniedException();
        }
        
        foreach (var filter in Filters) {
            if (!await filter.CheckCanRead(userId.Value, entity)) {
                return false;
            }
        }
        return true;
    }
}