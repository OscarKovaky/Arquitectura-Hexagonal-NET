using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Queries;
using Prueba.Core.Dtos;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class GetUserQueryHandler: IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUsersQuery> _logger;
    public GetUserQueryHandler(IUserService userService, ILogger<GetUsersQuery> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to retrieve all users");
        
        var users  =  await _userService.GetAllUsersAsync();
        
        _logger.LogInformation("Successfully retrieved {UserCount} users", users.Count());

        return users;
    }
}