using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Commands;

public class LoginUserCommand : IRequest<string>
{
    public LoginUserDto UserDto { get; set; }
}