using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models.Sections;
using Microsoft.Data.SqlClient;

namespace Duo.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly DatabaseConnection databaseConnection;

    public SectionRepository(DatabaseConnection databaseConnection)
    {
        ArgumentNullException.ThrowIfNull(databaseConnection);
        this.databaseConnection = databaseConnection;
    }

    public async Task<List<Section>> GetAllAsync()
    {
        try
        {
            var sections = new List<Section>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetAllSections";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                sections.Add(new Section
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    RoadmapId = reader.GetInt32(reader.GetOrdinal("RoadmapId")),
                    OrderNumber = reader.GetInt32(reader.GetOrdinal("OrderNumber"))
                });
            }

            return sections;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving sections: {ex.Message}", ex);
        }
    }

    public async Task<Section> GetByIdAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetSectionById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Section
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    RoadmapId = reader.GetInt32(reader.GetOrdinal("RoadmapId")),
                    OrderNumber = reader.GetInt32(reader.GetOrdinal("OrderNumber"))
                };
            }

            throw new KeyNotFoundException($"Section with ID {sectionId} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving section with ID {sectionId}: {ex.Message}", ex);
        }
    }

    public async Task<List<Section>> GetByRoadmapIdAsync(int roadmapId)
    {
        if (roadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(roadmapId));
        }

        try
        {
            var sections = new List<Section>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetSectionsByRoadmapId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapId", roadmapId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                sections.Add(new Section
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    RoadmapId = reader.GetInt32(reader.GetOrdinal("RoadmapId")),
                    OrderNumber = reader.GetInt32(reader.GetOrdinal("OrderNumber"))
                });
            }

            return sections;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving sections for roadmap {roadmapId}: {ex.Message}", ex);
        }
    }

    public async Task<int> LastOrderNumberByRoadmapIdAsync(int roadmapId)
    {
        if (roadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(roadmapId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_LastOrderSectionByRoadmapId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapId", roadmapId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? reader.GetInt32(reader.GetOrdinal("LastOrderNumber")) : 0;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving last order number for roadmap {roadmapId}: {ex.Message}", ex);
        }
    }

    public async Task<int> CountByRoadmapIdAsync(int roadmapId)
    {
        if (roadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(roadmapId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_CountSectionsByRoadmapId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@roadmapId", roadmapId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? reader.GetInt32(reader.GetOrdinal("SectionCount")) : 0;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving section count for roadmap {roadmapId}: {ex.Message}", ex);
        }
    }

    public async Task<int> AddAsync(Section section)
    {
        ArgumentNullException.ThrowIfNull(section);

        if (string.IsNullOrWhiteSpace(section.Title))
        {
            throw new ArgumentException("Section title cannot be null or empty.", nameof(section));
        }

        if (section.RoadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(section));
        }

        if (section.OrderNumber < 0)
        {
            throw new ArgumentException("Order number cannot be negative.", nameof(section));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_AddSection";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@subjectId", section.SubjectId);
            command.Parameters.AddWithValue("@title", section.Title);
            command.Parameters.AddWithValue("@description", section.Description);
            command.Parameters.AddWithValue("@roadmapId", section.RoadmapId);
            command.Parameters.AddWithValue("@orderNumber", section.OrderNumber);

            var newIdParam = new SqlParameter("@newId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(newIdParam);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return (int)newIdParam.Value;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while adding section '{section.Title}': {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(Section section)
    {
        ArgumentNullException.ThrowIfNull(section);

        if (section.Id <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(section));
        }

        if (string.IsNullOrWhiteSpace(section.Title))
        {
            throw new ArgumentException("Section title cannot be null or empty.", nameof(section));
        }

        if (section.RoadmapId <= 0)
        {
            throw new ArgumentException("Roadmap ID must be greater than 0.", nameof(section));
        }

        if (section.OrderNumber < 0)
        {
            throw new ArgumentException("Order number cannot be negative.", nameof(section));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_UpdateSection";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", section.Id);
            command.Parameters.AddWithValue("@subjectId", section.SubjectId);
            command.Parameters.AddWithValue("@title", section.Title);
            command.Parameters.AddWithValue("@description", section.Description);
            command.Parameters.AddWithValue("@roadmapId", section.RoadmapId);
            command.Parameters.AddWithValue("@orderNumber", section.OrderNumber);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating section '{section.Title}': {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_DeleteSection";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting section with ID {sectionId}: {ex.Message}", ex);
        }
    }
}
