using MediatR;

namespace Prueba.Application.Commands;

public class DeleteTreeNodeCommand : IRequest<Unit>
{
    public int Id { get; set; }
    
    public DeleteTreeNodeCommand(int id)
    {
        Id = id;
    }
}