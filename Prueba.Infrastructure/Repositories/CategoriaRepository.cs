using Dapper;
using Prueba.Core.Dtos;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;
using Prueba.Infrastructure.Persistence;

namespace Prueba.Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly DatabaseContext _context;

    public CategoriaRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        using (var connection = _context.CreateConnection())
        {
            // Llamada al procedimiento almacenado 'get_all_categorias'
            return await connection.QueryAsync<CategoriaDto>("SELECT * FROM public.get_all_categorias()");
        }
    }
}