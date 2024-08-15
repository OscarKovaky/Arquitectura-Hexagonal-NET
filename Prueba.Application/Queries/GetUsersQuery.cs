using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
    
}