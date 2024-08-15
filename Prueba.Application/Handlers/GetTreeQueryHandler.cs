using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Queries;
using Prueba.Core.Dtos;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class GetTreeQueryHandler : IRequestHandler<GetTreeQuery, IEnumerable<TreeNodeDto>>
{
    private readonly ITreeService _treeNodeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTreeQueryHandler> _logger;

    public GetTreeQueryHandler(ITreeService treeNodeRepository, IMapper mapper, ILogger<GetTreeQueryHandler> logger)
    {
        _treeNodeRepository = treeNodeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TreeNodeDto>> Handle(GetTreeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching the complete tree structure");

        var nodes = await _treeNodeRepository.GetTreeAsync();
        var nodeDtos = _mapper.Map<IEnumerable<TreeNodeDto>>(nodes);

        _logger.LogInformation("Tree structure fetched successfully with {NodeCount} nodes", nodeDtos.Count());

        return nodeDtos;
    }
}