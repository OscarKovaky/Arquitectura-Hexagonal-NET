using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Queries;

public class GetTreeQueryUser: IRequest<IEnumerable<TreeNodeDto>>
{
    public int Id { get; set; }
    
    public GetTreeQueryUser(int id)
    {
        Id = id;
    }
}