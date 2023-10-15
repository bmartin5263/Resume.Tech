using ResumeTech.Common.Auth;

namespace ResumeTech.Common.Actions; 

public interface IUnitOfWorkFactory {
    IUnitOfWorkDisposable Create(UserDetails userDetails);
}