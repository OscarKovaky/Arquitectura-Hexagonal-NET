using MediatR;

namespace Prueba.Application.Commands;

public class UpdateUsernameCommand: IRequest<Unit>
{
    public int Id { get; set; }
    public string Username { get; set; }
}