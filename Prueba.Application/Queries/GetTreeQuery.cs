using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Queries;

public class GetTreeQuery: IRequest<IEnumerable<TreeNodeDto>>
{
    
}