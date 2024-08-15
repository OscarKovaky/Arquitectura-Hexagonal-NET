using MediatR;

namespace Prueba.Application.Commands;

public class UpdateTreeNodeCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsFile { get; set; }
    public string Path { get; set; }
    public int? ParentId { get; set; }
}