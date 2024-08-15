using Prueba.Core.Entities;

namespace Prueba.Core.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<int> AddAsync(User entity);
    Task UpdateAsync(User entity);
    Task DeleteAsync(int id);
    Task<User> GetByUsernameAsync(string username);
}