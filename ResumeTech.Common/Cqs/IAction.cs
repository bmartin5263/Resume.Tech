using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Cqs; 

public interface IAction<I, O> {
    public Task<object?> Execute(object? args);
    public abstract string Name { get; }
    public virtual LogPolicy LogPolicy => LogPolicy.IncludeArguments;
    public virtual bool AllowAnonymous => false;
    public virtual IReadOnlySet<RoleName> RequiresAnyRole => ReadOnly.Set<RoleName>();
    public virtual IReadOnlySet<RoleName> RequiresAllRoles => ReadOnly.Set<RoleName>();
}