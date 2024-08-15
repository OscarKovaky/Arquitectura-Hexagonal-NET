using MediatR;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Prueba.Application.Handlers;

public class UpdateTreeNodeCommandHandler : IRequestHandler<UpdateTreeNodeCommand, Unit>
{
    private readonly ITreeNodeRepository _treeNodeRepository;

    private readonly ILogger<UpdateTreeNodeCommandHandler> _logger;

    public UpdateTreeNodeCommandHandler(ITreeNodeRepository treeNodeRepository, ILogger<UpdateTreeNodeCommandHandler> logger)
    {
        _treeNodeRepository = treeNodeRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateTreeNodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to update tree node with ID {NodeId}", request.Id);

        var node = await _treeNodeRepository.GetNodeByIdAsync(request.Id);
        if (node != null)
        {
            _logger.LogInformation("Tree node with ID {NodeId} found. Proceeding with update.", request.Id);

            node.Name = request.Name;
            node.IsFile = request.IsFile;
            node.Path = request.Path;
            node.ParentId = request.ParentId;

            await _treeNodeRepository.UpdateNodeAsync(node);

            _logger.LogInformation("Tree node with ID {NodeId} updated successfully", request.Id);
        }
        else
        {
            _logger.LogWarning("Tree node with ID {NodeId} not found. Update operation aborted.", request.Id);
        }

        return Unit.Value;
    }
}