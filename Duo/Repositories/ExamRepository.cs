using Duo.Data;
using Duo.Models.Quizzes;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duo.Repositories;

public class ExamRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public ExamRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IEnumerable<Exam>> GetAllAsync()
    {
        try
        {
            var exams = new List<Exam>();
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetAllExams";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                exams.Add(new Exam(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.IsDBNull(reader.GetOrdinal("SectionId")) ? null : reader.GetInt32(reader.GetOrdinal("SectionId"))
                ));
            }
            
            return exams;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exams: {ex.Message}", ex);
        }
    }

    public async Task<Exam> GetByIdAsync(int examId)
    {
        if (examId <= 0)
        {
            throw new ArgumentException("Exam ID must be greater than 0.", nameof(examId));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetExamById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@examId", examId);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                return new Exam(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.IsDBNull(reader.GetOrdinal("SectionId")) ? null : reader.GetInt32(reader.GetOrdinal("SectionId"))
                );
            }
            
            throw new KeyNotFoundException($"Exam with ID {examId} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exam with ID {examId}: {ex.Message}", ex);
        }
    }

    public async Task<Exam> GetBySectionIdAsync(int sectionId)
    {
        if (sectionId <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(sectionId));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetExamBySectionId";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sectionId", sectionId);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                return new Exam(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    sectionId
                );
            }
            
            throw new KeyNotFoundException($"Exam for section {sectionId} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exam for section {sectionId}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Exam>> GetUnassignedAsync()
    {
        try
        {
            var exams = new List<Exam>();
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetUnassignedExams";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                exams.Add(new Exam(
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    null
                ));
            }
            
            return exams;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving unassigned exams: {ex.Message}", ex);
        }
    }

    public async Task<int> AddAsync(Exam exam)
    {
        if (exam == null)
        {
            throw new ArgumentNullException(nameof(exam));
        }

        if (exam.SectionId.HasValue && exam.SectionId.Value <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(exam));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_AddExam";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            if (exam.SectionId.HasValue)
            {
                command.Parameters.AddWithValue("@sectionId", exam.SectionId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@sectionId", DBNull.Value);
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
            throw new Exception($"Database error while adding exam: {ex.Message}", ex);
        }
    }

    public async Task UpdateAsync(Exam exam)
    {
        if (exam == null)
        {
            throw new ArgumentNullException(nameof(exam));
        }

        if (exam.Id <= 0)
        {
            throw new ArgumentException("Exam ID must be greater than 0.", nameof(exam));
        }

        if (exam.SectionId.HasValue && exam.SectionId.Value <= 0)
        {
            throw new ArgumentException("Section ID must be greater than 0.", nameof(exam));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_UpdateExam";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@examId", exam.Id);
            
            if (exam.SectionId.HasValue)
            {
                command.Parameters.AddWithValue("@sectionId", exam.SectionId.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@sectionId", DBNull.Value);
            }
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while updating exam: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int examId)
    {
        if (examId <= 0)
        {
            throw new ArgumentException("Exam ID must be greater than 0.", nameof(examId));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_DeleteExam";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@examId", examId);
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting exam with ID {examId}: {ex.Message}", ex);
        }
    }
} 