using System.Net;
using ResumeTech.Common.Domain;
using ResumeTech.Common.Exceptions;

namespace ResumeTech.Application.Util; 

public static class ControllerUtils {
    
    public static TEntity OrElseNotFound<TEntity, ID>(this TEntity? self, ID id) {
        if (self == null) {
            throw new AppError(
                UserMessage: $"Not found by Id: {id}",
                StatusCode: HttpStatusCode.NotFound
            ).ToException();
        }
        return self;
    }
    
}