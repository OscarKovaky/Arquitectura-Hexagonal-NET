using System.Data;
using Dapper;
using Prueba.Core.Entities;
using Prueba.Core.Interfaces;
using Prueba.Infrastructure.Persistence;

namespace Prueba.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using (var connection = _context.CreateConnection())
        {
            // Llamada al procedimiento almacenado 'get_all_users'
            return await connection.QueryAsync<User>("SELECT * FROM public.get_all_users()");
        }
    }

    public async Task<User> GetByIdAsync(int id)
    {
        using (var connection =  _context.CreateConnection())
        {
            var sql = "SELECT * FROM public.GetUserById(@p_id)";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new {  p_id = id });
            return user;
        }
    }


    public async Task<int> AddAsync(User entity)
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = "SELECT public.adduser(@p_username, @p_passwordhash, @p_email, @p_role)";
            var userId = await connection.ExecuteScalarAsync<int>(sql, new 
            { 
                p_username = entity.Username, 
                p_passwordhash = entity.PasswordHash, 
                p_email = entity.Email,
                p_role = entity.Role // Incluye role si es necesario
            });
            return userId;
        }
    }


    public async Task UpdateAsync(User entity)
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = "SELECT public.UpdateUser(@Id, @Username)";
            await connection.ExecuteAsync(sql, new 
            { 
                entity.Id, 
                entity.Username,
            });
        }
    }


    public async Task DeleteAsync(int id)
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = "SELECT public.DeleteUser(@id)";
            await connection.ExecuteAsync(sql, new {  id = id  });
        }
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = "SELECT * FROM public.GetUserByUsername(@Username)";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            return user;
        }
    }
}