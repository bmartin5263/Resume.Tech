using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions; 

public abstract class Action<I, O> {
    /// <summary>
    /// The name of this action used for logging and possibly serialization. Once a name is chosen and committed it
    /// should not change. The class name may change, but the Name cannot. Names should be in PascalCase. 
    /// </summary>
    public abstract string Name { get; }
    
    /// <summary>
    /// Specifies how arguments should be logged (if at all)
    /// </summary>
    public virtual LogPolicy LogPolicy => LogPolicy.IncludeArguments;
    
    /// <summary>
    /// Specifies what roles (any or all) that the logged in user must have
    /// </summary>
    public virtual Roles UserRoles => Roles.All(RoleName.User);
    
    /// <summary>
    /// Returns whether a logged in user is required for this Action. Implementation checks if there
    /// were any roles specified for this action, since every logged in user will have at least 1 role
    /// </summary>
    public bool RequiresLoggedInUser => !this.UserRoles.IsEmpty();
    
    public virtual Task Validate(ValidationContext<I> ctx) {
        return Task.CompletedTask;
    }
    
    public abstract Task<O> Run(I args);
}