using MediatR;

namespace Prueba.Application.Commands;

public class CreateTreeNodeCommand : IRequest<Unit>
{
    public string Name { get; set; }
    public bool IsFile { get; set; }
    public string Path { get; set; }
    public int? ParentId { get; set; }
    
    public int UserId { get; set; }
}