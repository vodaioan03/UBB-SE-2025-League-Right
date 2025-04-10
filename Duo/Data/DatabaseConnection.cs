using System;
using System.Threading.Tasks;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Duo.Data;

public class DatabaseConnection
{
    private readonly string connectionString;

    // INFO: best approach is to return a new connection each time
    private SqlConnection connection;

    public DatabaseConnection(IConfiguration configuration)
    {
        connectionString = configuration["DbConnection"];
        try
        {
            connection = new SqlConnection(connectionString);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to connect to the database", ex);
        }
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }

    public async Task<SqlConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(connectionString);
        return connection;
    }

    public SqlConnection GetConnection()
    {
        return connection;
    }

    public void Dispose()
    {
        connection?.Close();
        connection?.Dispose();
    }
}