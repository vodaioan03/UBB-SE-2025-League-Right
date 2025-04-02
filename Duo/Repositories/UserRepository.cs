using Duo.Data;
using Duo.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duo.Repositories;

public class UserRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public UserRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(username));
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetUserByUsername";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Username", username);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return new User(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Username")),
                reader.GetInt32(reader.GetOrdinal("LastCompletedSectionId")),
                reader.GetInt32(reader.GetOrdinal("LastCompletedQuizId"))
            );
        }
        
        throw new KeyNotFoundException($"User with username '{username}' not found.");
    }

    public async Task<int> CreateUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(user));
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_CreateUser";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Username", user.Username);
        
        var newIdParam = new SqlParameter("@newId", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        command.Parameters.Add(newIdParam);
        
        try
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return (int)newIdParam.Value;
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new InvalidOperationException($"Username '{user.Username}' already exists.", ex);
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while creating user '{user.Username}': {ex.Message}", ex);
        }
    }

    public async Task UpdateUserSectionProgressAsync(User user, int newLastSectionCompletedId)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (user.Id <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(user));
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_UpdateUserProgress";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserId", user.Id);
        command.Parameters.AddWithValue("@LastCompletedSectionId", newLastSectionCompletedId);
        command.Parameters.AddWithValue("@LastCompletedQuizId", DBNull.Value);
        
        try
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} not found.", ex);
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating user section progress: {ex.Message}", ex);
        }
    }

    public async Task UpdateUserQuizProgressAsync(User user, int newLastQuizCompletedId)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (user.Id <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(user));
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_UpdateUserProgress";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserId", user.Id);
        command.Parameters.AddWithValue("@LastCompletedSectionId", DBNull.Value);
        command.Parameters.AddWithValue("@LastCompletedQuizId", newLastQuizCompletedId);
        
        try
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} not found.", ex);
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating user quiz progress: {ex.Message}", ex);
        }
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetUserById";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@Id", userId);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return new User(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Username")),
                reader.GetInt32(reader.GetOrdinal("LastCompletedSectionId")),
                reader.GetInt32(reader.GetOrdinal("LastCompletedQuizId"))
            );
        }
        
        throw new KeyNotFoundException($"User with ID {userId} not found.");
    }

    public async Task IncrementUserProgressAsync(int userId)
    {

        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than 0.");
        }

        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
    
        command.CommandText = "sp_ProgressUserByOne";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserId", userId);
        try
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while incrementing user progress: {ex.Message}", ex);
        }
    }
}
