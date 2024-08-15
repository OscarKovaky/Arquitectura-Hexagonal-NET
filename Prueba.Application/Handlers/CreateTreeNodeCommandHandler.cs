using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class CreateTreeNodeCommandHandler : IRequestHandler<CreateTreeNodeCommand, Unit>
{
    private readonly ITreeNodeRepository _treeNodeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTreeNodeCommandHandler> _logger;

    public CreateTreeNodeCommandHandler(ITreeNodeRepository treeNodeRepository, IMapper mapper, ILogger<CreateTreeNodeCommandHandler> logger)
    {
        _treeNodeRepository = treeNodeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateTreeNodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new tree node with name {NodeName} for user {UserId}", request.Name, request.UserId);

        var node = _mapper.Map<TreeNode>(request);
        await _treeNodeRepository.AddNodeAsync(node);

        _logger.LogInformation("Tree node {NodeName} created successfully with ID {NodeId}", node.Name, node.Id);

        return Unit.Value;
    }
}