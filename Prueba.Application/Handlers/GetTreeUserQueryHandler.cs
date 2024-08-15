using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Queries;
using Prueba.Core.Dtos;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class GetTreeUserQueryHandler: IRequestHandler<GetTreeQueryUser, IEnumerable<TreeNodeDto>>
{
    private readonly ITreeService _treeService;

    private readonly ILogger<GetUserQueryHandler> _logger;

    public GetTreeUserQueryHandler(ITreeService treeService, ILogger<GetUserQueryHandler> logger)
    {
        _treeService = treeService;
        _logger = logger;
    }

    public async Task<IEnumerable<TreeNodeDto>> Handle(GetTreeQueryUser request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to retrieve all users");

        var users = await _treeService.GetUserNodes(request.Id);

        _logger.LogInformation("Successfully retrieved {UserCount} users", users.Count());

        return users;
    }
}