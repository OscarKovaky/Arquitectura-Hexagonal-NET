using Prueba.Core.Dtos;
using Prueba.Core.Entities;

namespace Prueba.Core.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<CategoriaDto>> GetAllAsync();
    
}