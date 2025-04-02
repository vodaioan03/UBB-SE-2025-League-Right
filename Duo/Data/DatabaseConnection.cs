using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Data.Common;

namespace Duo.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;

    // INFO: best approach is to return a new connection each time
    private SqlConnection _connection;

    public DatabaseConnection(IConfiguration configuration)
    {

        _connectionString = configuration["DbConnection"];

        // INFO: best approach is to return a new connection each time
        try {
            _connection = new SqlConnection(_connectionString);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to connect to the database", ex);
        }
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<SqlConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        return connection;
    }

    // INFO: best approach is to return a new connection each time
    public SqlConnection GetConnection()
    {
        return _connection;
    }

    // INFO: best approach is to return a new connection each time
    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}