using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Duo.Data;
using Duo.Models.Quizzes;
using Microsoft.Data.SqlClient;

namespace Duo.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly DatabaseConnection databaseConnection;

    public QuizRepository(DatabaseConnection databaseConnection)
    {
        this.databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<List<Quiz>> GetAllAsync()
    {
        try
        {
            var quizzes = new List<Quiz>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetAllQuizzes";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                quizzes.Add(new Quiz(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.IsDBNull(reader.GetOrdinal("SectionId")) ? null : reader.GetInt32(reader.GetOrdinal("SectionId")),
                    reader.IsDBNull(reader.GetOrdinal("OrderNumber")) ? null : reader.GetInt32(reader.GetOrdinal("OrderNumber"))));
            }

            return quizzes;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving quizzes: {ex.Message}", ex);
        }
    }

    public async Task<Quiz> GetByIdAsync(int quizId)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetQuizById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Quiz(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.IsDBNull(reader.GetOrdinal("SectionId")) ? null : reader.GetInt32(reader.GetOrdinal("SectionId")),
                    reader.IsDBNull(reader.GetOrdinal("OrderNumber")) ? null : reader.GetInt32(reader.GetOrdinal("OrderNumber")));
            }

            throw new KeyNotFoundException($"Quiz with ID {quizId} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving quiz with ID {quizId}: {ex.Message}", ex);
        }
    }

    public async Task<List<Quiz>> GetBySectionIdAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            var quizzes = new List<Quiz>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetQuizzesBySectionId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                quizzes.Add(new Quiz(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    sectionId,
                    reader.IsDBNull(reader.GetOrdinal("OrderNumber")) ? null : reader.GetInt32(reader.GetOrdinal("OrderNumber"))));
            }
            return quizzes;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving quizzes for section {sectionId}: {ex.Message}", ex);
        }
    }

    public async Task<List<Quiz>> GetUnassignedAsync()
    {
        try
        {
            var quizzes = new List<Quiz>();
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_GetUnassignedQuizzes";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                quizzes.Add(new Quiz(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    null,
                    reader.IsDBNull(reader.GetOrdinal("OrderNumber")) ? null : reader.GetInt32(reader.GetOrdinal("OrderNumber"))));
            }

            return quizzes;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving unassigned quizzes: {ex.Message}", ex);
        }
    }

    public async Task<int> AddAsync(Quiz quiz)
    {
        if (quiz == null)
        {
            throw new ArgumentNullException(nameof(quiz));
        }

        if (quiz.SectionId.HasValue && quiz.SectionId.Value <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(quiz));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_AddQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            if (quiz.SectionId.HasValue)
            {
                command.Parameters.AddWithValue("@sectionId", quiz.SectionId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@sectionId", DBNull.Value);
            }

            if (quiz.OrderNumber.HasValue)
            {
                command.Parameters.AddWithValue("@orderNumber", quiz.OrderNumber.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@orderNumber", DBNull.Value);
            }

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
            throw new Exception($"Database error while adding quiz: {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        if (quiz == null)
        {
            throw new ArgumentNullException(nameof(quiz));
        }

        if (quiz.Id <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quiz));
        }

        if (quiz.SectionId.HasValue && quiz.SectionId.Value <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(quiz));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_UpdateQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quiz.Id);

            if (quiz.SectionId.HasValue)
            {
                command.Parameters.AddWithValue("@sectionId", quiz.SectionId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@sectionId", DBNull.Value);
            }

            if (quiz.OrderNumber.HasValue)
            {
                command.Parameters.AddWithValue("@orderNumber", quiz.OrderNumber.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@orderNumber", DBNull.Value);
            }

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating quiz: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int quizId)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_DeleteQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting quiz with ID {quizId}: {ex.Message}", ex);
        }
    }

    public async Task AddExerciseToQuiz(int quizId, int exerciseId)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        if (exerciseId <= 0)
        {
            throw new ArgumentException("Exercise ID must be greater than 0.", nameof(exerciseId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_AddExerciseToQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);
            command.Parameters.AddWithValue("@exerciseId", exerciseId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while adding exercise to quiz: {ex.Message}", ex);
        }
    }

    public async Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        if (exerciseId <= 0)
        {
            throw new ArgumentException("Exercise ID must be greater than 0.", nameof(exerciseId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_RemoveExerciseFromQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);
            command.Parameters.AddWithValue("@exerciseId", exerciseId);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while removing exercise from quiz: {ex.Message}", ex);
        }
    }

    public async Task UpdateQuizSection(int quizId, int? sectionId, int? orderNumber = null)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        if (sectionId.HasValue && sectionId.Value <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_UpdateQuiz";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);
            command.Parameters.AddWithValue("@sectionId", sectionId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@orderNumber", orderNumber ?? (object)DBNull.Value);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating quiz section: {ex.Message}", ex);
        }
    }

    public async Task<int> LastOrderNumberBySectionIdAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            var lastOrderNumber = 0;
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_LastOrderQuizBySectionId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                lastOrderNumber = reader.GetInt32(reader.GetOrdinal("LastOrderNumber"));
            }

            return lastOrderNumber;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving last order number for section {sectionId}: {ex.Message}", ex);
        }
    }

    public async Task<int> CountBySectionIdAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            var countOfQuizzes = 0;
            using var connection = await databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();

            command.CommandText = "sp_CountQuizzesBySectionId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                countOfQuizzes = reader.GetInt32(reader.GetOrdinal("QuizCount"));
            }

            return countOfQuizzes;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving quiz count for section {sectionId}: {ex.Message}", ex);
        }
    }
}