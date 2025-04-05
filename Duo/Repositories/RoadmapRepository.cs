using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models.Roadmap;
using Microsoft.Data.SqlClient;

namespace Duo.Repositories;

public class RoadmapRepository : IRoadmapRepository
{
    private readonly DatabaseConnection databaseConnection;

    public RoadmapRepository(DatabaseConnection databaseConnection)
    {
        ArgumentNullException.ThrowIfNull(databaseConnection);
        this.databaseConnection = databaseConnection;
    }

    public async Task<List<Roadmap>> GetAllAsync()
    {
        try
        {
            var roadmaps = new List<Roadmap>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetRoadmaps";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roadmaps.Add(new Roadmap
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                });
            }

            return roadmaps;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving roadmaps: {ex.Message}", ex);
        }
    }

    public async Task<Roadmap> GetByIdAsync(int roadmapId)
    {
        if (roadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(roadmapId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetRoadmapById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapId", roadmapId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Roadmap
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                };
            }

            throw new KeyNotFoundException($"Roadmap with ID {roadmapId} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving roadmap with ID {roadmapId}: {ex.Message}", ex);
        }
    }

    public async Task<Roadmap> GetByNameAsync(string roadmapName)
    {
        if (string.IsNullOrWhiteSpace(roadmapName))
        {
            throw new ArgumentException("Roadmap name cannot be null or empty.", nameof(roadmapName));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetRoadmapByName";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapName", roadmapName);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Roadmap
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                };
            }

            throw new KeyNotFoundException($"Roadmap with name '{roadmapName}' not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving roadmap with name '{roadmapName}': {ex.Message}", ex);
        }
    }

    public async Task<int> AddAsync(Roadmap roadmap)
    {
        ArgumentNullException.ThrowIfNull(roadmap);

        if (string.IsNullOrWhiteSpace(roadmap.Name))
        {
            throw new ArgumentException("Roadmap name cannot be null or empty.", nameof(roadmap));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_AddRoadmap";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", roadmap.Name);

            var newIdParam = new SqlParameter("@newId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output,
            };
            command.Parameters.Add(newIdParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return (int)newIdParam.Value;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while adding roadmap '{roadmap.Name}': {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int roadmapId)
    {
        if (roadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(roadmapId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_DeleteRoadmap";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapId", roadmapId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting roadmap with ID {roadmapId}: {ex.Message}", ex);
        }
    }
}
