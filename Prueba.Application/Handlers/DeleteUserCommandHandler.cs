using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand,Unit>
{
    private readonly IUserRepository _userRepository;

    private readonly ILogger<DeleteTreeNodeCommandHandler> _logger;

    public DeleteUserCommandHandler(IUserRepository userRepository, ILogger<DeleteTreeNodeCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tree node with ID {NodeId}", request.Id);
        
        await _userRepository.DeleteAsync(request.Id);
        
        _logger.LogInformation("Tree node with ID {NodeId} deleted successfully", request.Id);
        return Unit.Value;
    }
}