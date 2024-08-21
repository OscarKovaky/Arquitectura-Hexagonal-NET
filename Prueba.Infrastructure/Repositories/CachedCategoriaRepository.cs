using Prueba.Core.Dtos;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;
using Prueba.Core.Services;

namespace Prueba.Infrastructure.Repositories;

public class CachedCategoriaRepository : ICategoriaRepository
{
    private readonly CategoriaRepository _categoriaRepository;
    private readonly ICacheService _cacheService;
    private readonly string _cacheKey = "CategoriasCache";

    public CachedCategoriaRepository(CategoriaRepository categoriaRepository, ICacheService cacheService)
    {
        _categoriaRepository = categoriaRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        var cachedCategorias = await _cacheService.GetAsync<IEnumerable<CategoriaDto>>(_cacheKey);

        if (cachedCategorias != null)
        {
            return cachedCategorias;
        }

        var categorias = await _categoriaRepository.GetAllAsync();
        await _cacheService.SetAsync(_cacheKey, categorias, TimeSpan.FromMinutes(30));

        return categorias;
    }
}