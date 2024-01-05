using MediatR;

namespace ResumeTech.Common.Mediatr;

public interface ICommand : IRequest {
    
}

public class Handler : IRequestHandler<ICommand> {
    
    public Task Handle(ICommand request, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
    
}