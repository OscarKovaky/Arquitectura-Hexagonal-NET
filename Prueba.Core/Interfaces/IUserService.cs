using Prueba.Core.Dtos;

namespace Prueba.Core.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(UserDto newUser);
    Task UpdateUserAsync(UserDto updatedUser);
    Task DeleteUserAsync(int id);
}