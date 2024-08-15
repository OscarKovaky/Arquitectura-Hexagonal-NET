using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class UpdateUsernameCommandHandler: IRequestHandler<UpdateUsernameCommand,Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateUsernameCommandHandler> _logger;

    public UpdateUsernameCommandHandler(IUserRepository userRepository, ILogger<UpdateUsernameCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting update process for user ID {UserId}", request.Id);

        // Fetch the user from the repository
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} not found.");
        }

        // Update the username
        user.Username = request.Username;

        // Persist the changes
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Username updated successfully for user ID {UserId}", request.Id);

        return Unit.Value;
    }
}