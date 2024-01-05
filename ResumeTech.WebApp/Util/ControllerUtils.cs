using System.Net;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Error;

namespace ResumeTech.Application.Util; 

public static class ControllerUtils {
    
    public static TEntity OrElseNotFound<TEntity, ID>(this TEntity? self, ID id) where ID : notnull {
        if (self == null) {
            throw EntityNotFoundException.IdNotFound<TEntity>(id);
        }
        return self;
    }
    
}