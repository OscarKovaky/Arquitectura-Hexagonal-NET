using AutoMapper;
using MediatR;
using Prueba.Application.Queries.Categorias;
using Prueba.Core.Dtos;
using Prueba.Core.Interfaces;

namespace Prueba.Application.Handlers.Categorias;

public class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, IEnumerable<CategoriaDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IMapper _mapper;

    public GetCategoriasQueryHandler(ICategoriaRepository categoriaRepository, IMapper mapper)
    {
        _categoriaRepository = categoriaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _categoriaRepository.GetAllAsync();
        return categorias;
    }
}