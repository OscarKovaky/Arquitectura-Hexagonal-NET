using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Commands;

public class RegisterUserCommand : IRequest<int>
{
    public RegisterUserDto UserDto { get; set; }
}
