﻿using Duo.Data;
using Duo.Models;
using Duo.Models.Exercises;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Duo.Repositories;

public class ExerciseRepository
{
    private readonly DatabaseConnection _databaseConnection;

    public ExerciseRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
    {
        try
        {
            var exercises = new List<Exercise>();
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetAllExercises";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var type = reader.GetString(reader.GetOrdinal("Type"));
                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                var question = reader.GetString(reader.GetOrdinal("Question"));
                var difficulty = (Difficulty)reader.GetInt32(reader.GetOrdinal("DifficultyId"));

                Exercise exercise = type switch
                {
                    "MultipleChoice" => await GetMultipleChoiceExerciseAsync(id, question, difficulty),
                    "FillInTheBlanks" => await GetFillInTheBlanksExerciseAsync(id, question, difficulty),
                    "Association" => await GetAssociationExerciseAsync(id, question, difficulty),
                    _ => throw new InvalidOperationException($"Unknown exercise type: {type}")
                };

                exercises.Add(exercise);
            }
            
            return exercises;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exercises: {ex.Message}", ex);
        }
    }

    public async Task<Exercise> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Exercise ID must be greater than 0.", nameof(id));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetExerciseById";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@exerciseId", id);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                var type = reader.GetString(reader.GetOrdinal("Type"));
                var question = reader.GetString(reader.GetOrdinal("Question"));
                var difficulty = (Difficulty)reader.GetInt32(reader.GetOrdinal("DifficultyId"));

                return type switch
                {
                    "MultipleChoice" => await GetMultipleChoiceExerciseAsync(id, question, difficulty),
                    "FillInTheBlanks" => await GetFillInTheBlanksExerciseAsync(id, question, difficulty),
                    "Association" => await GetAssociationExerciseAsync(id, question, difficulty),
                    _ => throw new InvalidOperationException($"Unknown exercise type: {type}")
                };
            }
            
            throw new KeyNotFoundException($"Exercise with ID {id} not found.");
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exercise with ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Exercise>> GetQuizExercisesAsync(int quizId)
    {
        if (quizId <= 0)
        {
            throw new ArgumentException("Quiz ID must be greater than 0.", nameof(quizId));
        }

        try
        {
            var exercises = new List<Exercise>();
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetQuizExercises";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@quizId", quizId);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var type = reader.GetString(reader.GetOrdinal("Type"));
                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                var question = reader.GetString(reader.GetOrdinal("Question"));
                var difficulty = (Difficulty)reader.GetInt32(reader.GetOrdinal("DifficultyId"));

                Exercise exercise = type switch
                {
                    "MultipleChoice" => await GetMultipleChoiceExerciseAsync(id, question, difficulty),
                    "FillInTheBlanks" => await GetFillInTheBlanksExerciseAsync(id, question, difficulty),
                    "Association" => await GetAssociationExerciseAsync(id, question, difficulty),
                    _ => throw new InvalidOperationException($"Unknown exercise type: {type}")
                };

                exercises.Add(exercise);
            }
            
            return exercises;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exercises for quiz {quizId}: {ex.Message}", ex);
        }
    }

    public async Task<int> AddExerciseAsync(Exercise exercise)
    {
        if (exercise == null)
        {
            throw new ArgumentNullException(nameof(exercise));
        }

        if (string.IsNullOrWhiteSpace(exercise.Question))
        {
            throw new ArgumentException("Exercise question cannot be empty.", nameof(exercise));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_AddExercise";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@type", exercise.GetType().Name);
            command.Parameters.AddWithValue("@difficultyId", (int)exercise.Difficulty);
            
            // Initialize all optional parameters as NULL
            command.Parameters.AddWithValue("@question", DBNull.Value);
            command.Parameters.AddWithValue("@correctAnswer", DBNull.Value);
            command.Parameters.AddWithValue("@sentence", DBNull.Value);
            command.Parameters.AddWithValue("@flashcardSentence", DBNull.Value);
            command.Parameters.AddWithValue("@flashcardAnswer", DBNull.Value);
            command.Parameters.AddWithValue("@timeInSeconds", DBNull.Value);
            
            var newIdParam = new SqlParameter("@newId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(newIdParam);
            
            // Set type-specific parameters
            switch (exercise)
            {
                case MultipleChoiceExercise mcExercise:
                    var correctAnswer = mcExercise.Choices.FirstOrDefault(c => c.IsCorrect)?.Answer;
                    if (string.IsNullOrEmpty(correctAnswer))
                    {
                        throw new ArgumentException("Multiple choice exercise must have one correct answer.");
                    }
                    command.Parameters["@question"].Value = mcExercise.Question;
                    command.Parameters["@correctAnswer"].Value = correctAnswer;
                    break;

                case FillInTheBlankExercise fbExercise:
                    command.Parameters["@sentence"].Value = fbExercise.Question;
                    break;

                case AssociationExercise:
                    // No additional parameters needed for association exercises
                    break;

                default:
                    throw new ArgumentException($"Unsupported exercise type: {exercise.GetType().Name}");
            }
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var newId = (int)newIdParam.Value;

            // Add additional data based on exercise type
            switch (exercise)
            {
                case MultipleChoiceExercise mcExercise:
                    // Add wrong answers as options
                    foreach (var choice in mcExercise.Choices.Where(c => !c.IsCorrect))
                    {
                        using var optionCommand = connection.CreateCommand();
                        optionCommand.CommandText = "sp_AddMultipleChoiceOption";
                        optionCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        optionCommand.Parameters.AddWithValue("@exerciseId", newId);
                        optionCommand.Parameters.AddWithValue("@optionText", choice.Answer);
                        await optionCommand.ExecuteNonQueryAsync();
                    }
                    break;

                case FillInTheBlankExercise fbExercise:
                    // Add all possible correct answers
                    foreach (var answer in fbExercise.PossibleCorrectAnswers)
                    {
                        using var answerCommand = connection.CreateCommand();
                        answerCommand.CommandText = "sp_AddFillInTheBlanksAnswer";
                        answerCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        answerCommand.Parameters.AddWithValue("@exerciseId", newId);
                        answerCommand.Parameters.AddWithValue("@correctAnswer", answer);
                        await answerCommand.ExecuteNonQueryAsync();
                    }
                    break;

                case AssociationExercise assocExercise:
                    // Add all pairs
                    for (int i = 0; i < assocExercise.FirstAnswersList.Count; i++)
                    {
                        using var pairCommand = connection.CreateCommand();
                        pairCommand.CommandText = "sp_AddAssociationPair";
                        pairCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        pairCommand.Parameters.AddWithValue("@exerciseId", newId);
                        pairCommand.Parameters.AddWithValue("@firstAnswer", assocExercise.FirstAnswersList[i]);
                        pairCommand.Parameters.AddWithValue("@secondAnswer", assocExercise.SecondAnswersList[i]);
                        await pairCommand.ExecuteNonQueryAsync();
                    }
                    break;
            }
            
            return newId;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while creating exercise: {ex.Message}", ex);
        }
    }

    public async Task DeleteExerciseAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Exercise ID must be greater than 0.", nameof(id));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_DeleteExercise";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@exerciseId", id);
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new KeyNotFoundException($"Exercise with ID {id} not found.", ex);
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while deleting exercise with ID {id}: {ex.Message}", ex);
        }
    }

    private async Task<MultipleChoiceExercise> GetMultipleChoiceExerciseAsync(int id, string question, Difficulty difficulty)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetMultipleChoiceExercise";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@exerciseId", id);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        var choices = new List<MultipleChoiceAnswerModel>();
        
        if (await reader.ReadAsync())
        {
            choices.Add(new MultipleChoiceAnswerModel
            {
                Answer = reader.GetString(reader.GetOrdinal("CorrectAnswer")),
                IsCorrect = true
            });
        }
        
        while (await reader.ReadAsync())
        {
            choices.Add(new MultipleChoiceAnswerModel
            {
                Answer = reader.GetString(reader.GetOrdinal("OptionText")),
                IsCorrect = false
            });
        }
        
        return new MultipleChoiceExercise(id, question, difficulty, choices);
    }

    private async Task<FillInTheBlankExercise> GetFillInTheBlanksExerciseAsync(int id, string question, Difficulty difficulty)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetFillInTheBlanksExercise";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@exerciseId", id);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        var answers = new List<string>();
        while (await reader.ReadAsync())
        {
            answers.Add(reader.GetString(reader.GetOrdinal("Answer")));
        }
        
        return new FillInTheBlankExercise(id, question, difficulty, answers);
    }

    private async Task<AssociationExercise> GetAssociationExerciseAsync(int id, string question, Difficulty difficulty)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetAssociationExercise";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@exerciseId", id);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        var firstAnswers = new List<string>();
        var secondAnswers = new List<string>();
        while (await reader.ReadAsync())
        {
            firstAnswers.Add(reader.GetString(reader.GetOrdinal("FirstAnswer")));
            secondAnswers.Add(reader.GetString(reader.GetOrdinal("SecondAnswer")));
        }
        
        return new AssociationExercise(id, question, difficulty, firstAnswers, secondAnswers);
    }
}
