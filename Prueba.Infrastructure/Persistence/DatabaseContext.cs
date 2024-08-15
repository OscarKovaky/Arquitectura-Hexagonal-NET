using System.Data;
using Npgsql;

namespace Prueba.Infrastructure.Persistence;

public class DatabaseContext
{ 
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}