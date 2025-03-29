using Duo.Data;
using Duo.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace Duo.Repositories;

public class UserRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public UserRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "SELECT Id, Username, LastCompletedSectionId, LastCompletedQuizId FROM Users WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (reader.Read())
        {
            return new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3)
            );
        }
        
        return null;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = @"
            INSERT INTO Users (Username, LastCompletedSectionId, LastCompletedQuizId) 
            VALUES (@Username, @LastCompletedSectionId, @LastCompletedQuizId)";
        
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@LastCompletedSectionId", user.LastCompletedSectionId);
        command.Parameters.AddWithValue("@LastCompletedQuizId", user.LastCompletedQuizId);
        
        try
        {
            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
        catch (SqlException)
        {
            return false;
        }
    }

    public async Task UpdateUserSectionProgressAsync(User user, int newLastSectionCompletedId)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "UPDATE Users SET LastCompletedSectionId = @SectionId WHERE Id = @UserId";
        command.Parameters.AddWithValue("@SectionId", newLastSectionCompletedId);
        command.Parameters.AddWithValue("@UserId", user.Id);
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateUserQuizProgressAsync(User user, int newLastQuizCompletedId)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "UPDATE Users SET LastCompletedQuizId = @QuizId WHERE Id = @UserId";
        command.Parameters.AddWithValue("@QuizId", newLastQuizCompletedId);
        command.Parameters.AddWithValue("@UserId", user.Id);
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "SELECT Id, Username, LastCompletedSectionId, LastCompletedQuizId FROM Users WHERE Id = @UserId";
        command.Parameters.AddWithValue("@UserId", userId);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetInt32(3)
            );
        }
        
        return null;
    }
}
