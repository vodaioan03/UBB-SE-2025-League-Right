using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;

namespace Duo.Core.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;
    public DatabaseConnection(IConfiguration configuration)
    {
        var dataSource = configuration["LocalDataSource"];
        var catalog = configuration["InitialCatalog"];
        
        if (string.IsNullOrEmpty(dataSource) || string.IsNullOrEmpty(catalog))
        {
            throw new InvalidOperationException("Database configuration is missing");
        }

        _connectionString = $"Server={dataSource};Database={catalog};Trusted_Connection=True;TrustServerCertificate=True;";
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}