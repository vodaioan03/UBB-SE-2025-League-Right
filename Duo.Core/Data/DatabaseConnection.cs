using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;

namespace Duo.Core.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;

    // INFO: best approach is to return a new connection each time
    private SqlConnection _connection;

    public DatabaseConnection(IConfiguration configuration)
    {
        var dataSource = configuration["DataSource"];
        var catalog = configuration["InitialCatalog"];
        var userId = configuration["UserId"];
        var password = configuration["Password"];

        if (string.IsNullOrEmpty(dataSource) || string.IsNullOrEmpty(catalog) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Database configuration is missing");
        }

        _connectionString = $"Server={dataSource};Database={catalog};User Id={userId};Password={password};TrustServerCertificate=True;";

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