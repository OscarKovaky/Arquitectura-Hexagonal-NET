using MediatR;

namespace Prueba.Application.Commands;

public class DeleteTreeNodeAndChildrenCommand : IRequest<Unit>
{
    public int Id { get; set; }
    
    public DeleteTreeNodeAndChildrenCommand(int id)
    {
        Id = id;
    }
}