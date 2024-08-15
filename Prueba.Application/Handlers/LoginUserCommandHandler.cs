using MediatR;
using Microsoft.Extensions.Logging;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, ILogger<LoginUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to log in user {Username}", request.UserDto.Username);

        var user = await _userRepository.GetByUsernameAsync(request.UserDto.Username);

        if (user is null)
        {
            _logger.LogWarning("Login failed: User {Username} not found", request.UserDto.Username);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.UserDto.Password))
        {
            _logger.LogWarning("Login failed: Incorrect password for user {Username}", request.UserDto.Username);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        _logger.LogInformation("User {Username} logged in successfully. Generating JWT token.", request.UserDto.Username);
        var token = _jwtTokenGenerator.GenerateToken(user);

        _logger.LogInformation("JWT token generated successfully for user {Username}", request.UserDto.Username);

        return token;
    }
}