using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class DeleteTreeNodeCommandHandler : IRequestHandler<DeleteTreeNodeCommand,Unit>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    private readonly ILogger<DeleteTreeNodeCommandHandler> _logger;

    public DeleteTreeNodeCommandHandler(ITreeNodeRepository treeNodeRepository, ILogger<DeleteTreeNodeCommandHandler> logger)
    {
        _treeNodeRepository = treeNodeRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteTreeNodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tree node with ID {NodeId}", request.Id);

        await _treeNodeRepository.DeleteNodeAsync(request.Id);

        _logger.LogInformation("Tree node with ID {NodeId} deleted successfully", request.Id);

        return Unit.Value;
    }
}