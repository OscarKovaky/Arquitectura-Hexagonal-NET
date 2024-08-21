using MediatR;
using Prueba.Core.Dtos;

namespace Prueba.Application.Queries.Categorias;

public class GetCategoriasQuery : IRequest<IEnumerable<CategoriaDto>>
{
}