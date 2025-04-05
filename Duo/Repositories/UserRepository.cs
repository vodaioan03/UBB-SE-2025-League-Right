using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models;
using Microsoft.Data.SqlClient;

namespace Duo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public UserRepository(DatabaseConnection databaseConnection)
        {
            ArgumentNullException.ThrowIfNull(databaseConnection);
            this.databaseConnection = databaseConnection;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetUserByUsername";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetString(reader.GetOrdinal("Username")),
                    reader.GetInt32(reader.GetOrdinal("NumberOfCompletedSections")),
                    reader.GetInt32(reader.GetOrdinal("NumberOfCompletedQuizzesInSection")));
            }

            throw new KeyNotFoundException($"User with username '{username}' not found.");
        }

        public async Task<int> CreateUserAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(user));
            }

            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_CreateUser";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", user.Username);

            var newIdParam = new SqlParameter("@newId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
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

        public async Task UpdateUserProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesCompletedInSection)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_UpdateUserProgress";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@NumberOfCompletedSections", newNrOfSectionsCompleted);
            command.Parameters.AddWithValue("@NumberOfCompletedQuizzesInSection", newNrOfQuizzesCompletedInSection);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error while updating user section progress: {ex.Message}", ex);
            }
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetUserById";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@userId", userId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetString(reader.GetOrdinal("Username")),
                    reader.GetInt32(reader.GetOrdinal("NumberOfCompletedSections")),
                    reader.GetInt32(reader.GetOrdinal("NumberOfCompletedQuizzesInSection")));
            }

            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.");
            }

            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_ProgressUserByOne";
            command.CommandType = CommandType.StoredProcedure;
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
}
