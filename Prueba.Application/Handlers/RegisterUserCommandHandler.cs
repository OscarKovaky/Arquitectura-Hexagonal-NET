using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ILogger<RegisterUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting registration process for user {Username}", request.UserDto.Username);

        var user = new User
        {
            Username = request.UserDto.Username,
            PasswordHash = _passwordHasher.HashPassword(request.UserDto.Password),
            Email = request.UserDto.Email,
            Role = request.UserDto.Role
        };

        var userId = await _userRepository.AddAsync(user);

        _logger.LogInformation("User {Username} registered successfully with ID {UserId}", request.UserDto.Username, userId);

        return userId;
    }
}