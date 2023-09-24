using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions; 

public abstract class Action<I, O> {
    public abstract string Name { get; }
    public virtual LogPolicy LogPolicy => LogPolicy.IncludeArguments;
    public virtual RoleName[] Roles => new[] { RoleName.User };
    public virtual bool RequiresLoggedInUser => Roles.Length > 0;
    
    public virtual Task Validate(UserDetails user, I args) {
        return Task.CompletedTask;
    }
    
    public abstract Task<O> Run(I args);
}