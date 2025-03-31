using Duo.Data;
using Duo.Models;
using Duo.Models.Exercises;
using Duo.Models.Quizzes;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    "Flashcard" => await GetFlashcardExerciseAsync(id, question, difficulty),
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
                    "Flashcard" => await GetFlashcardExerciseAsync(id, question, difficulty),
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
            
            command.CommandText = "sp_GetQuizByIdWithExercises";
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
                    "Flashcard" => await GetFlashcardExerciseAsync(id, question, difficulty),
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

    public async Task<IEnumerable<Exercise>> GetExamExercisesAsync(int examId)
    {
        if (examId <= 0)
        {
            throw new ArgumentException("Exam ID must be greater than 0.", nameof(examId));
        }

        try
        {
            var exercises = new List<Exercise>();
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_GetExamByIdWithExercises";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@examId", examId);
            
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
                    "Flashcard" => await GetFlashcardExerciseAsync(id, question, difficulty),
                    _ => throw new InvalidOperationException($"Unknown exercise type: {type}")
                };

                exercises.Add(exercise);
            }
            
            return exercises;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error while retrieving exercises for exam {examId}: {ex.Message}", ex);
        }
    }

    public async Task<int> AddExerciseAsync(Exercise exercise)
    {
        if (exercise == null)
        {
            throw new ArgumentNullException(nameof(exercise));
        }

        try
        {
            using var connection = await _databaseConnection.CreateConnectionAsync();
            using var command = connection.CreateCommand();
            
            command.CommandText = "sp_AddExercise";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@type", exercise.GetType().Name);
            Debug.WriteLine(exercise.GetType().Name);
            command.Parameters.AddWithValue("@difficultyId", (int)exercise.Difficulty);
            
            command.Parameters.AddWithValue("@question", DBNull.Value);
            command.Parameters.AddWithValue("@correctAnswer", DBNull.Value);
            command.Parameters.AddWithValue("@flashcardAnswer", DBNull.Value);
            command.Parameters.AddWithValue("@timeInSeconds", DBNull.Value);
            
            var newIdParam = new SqlParameter("@newId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(newIdParam);
            
            switch (exercise)
            {
                case MultipleChoiceExercise mcExercise:
                    if (string.IsNullOrWhiteSpace(mcExercise.Question))
                    {
                        throw new ArgumentException("Multiple choice exercise question cannot be empty.", nameof(exercise));
                    }
                    var correctAnswer = mcExercise.Choices.FirstOrDefault(c => c.IsCorrect)?.Answer;
                    if (string.IsNullOrEmpty(correctAnswer))
                    {
                        throw new ArgumentException("Multiple choice exercise must have one correct answer.");
                    }
                    command.Parameters["@question"].Value = mcExercise.Question;
                    command.Parameters["@correctAnswer"].Value = correctAnswer;
                    break;

                case FillInTheBlankExercise fbExercise:
                    if (string.IsNullOrWhiteSpace(fbExercise.Question))
                    {
                        throw new ArgumentException("Fill in the blank exercise question cannot be empty.", nameof(exercise));
                    }
                    command.Parameters["@question"].Value = fbExercise.Question;
                    break;

                case AssociationExercise assocExercise:
                    if (string.IsNullOrWhiteSpace(assocExercise.Question))
                    {
                        throw new ArgumentException("Association exercise question cannot be empty.", nameof(exercise));
                    }
                    command.Parameters["@question"].Value = assocExercise.Question;
                    break;

                case FlashcardExercise flashcardExercise:
                    if (string.IsNullOrWhiteSpace(flashcardExercise.Question))
                    {
                        throw new ArgumentException("Flashcard exercise question cannot be empty.", nameof(exercise));
                    }
                    command.Parameters["@question"].Value = flashcardExercise.Question;
                    command.Parameters["@flashcardAnswer"].Value = flashcardExercise.Answer;
                    break;

                default:
                    throw new ArgumentException($"Unsupported exercise type: {exercise.GetType().Name}");
            }
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            var newId = (int)newIdParam.Value;
            switch (exercise)
            {
                case MultipleChoiceExercise mcExercise:
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
            Debug.WriteLine(ex.Message);
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
            choices.Add(new MultipleChoiceAnswerModel(
                reader.GetString(reader.GetOrdinal("CorrectAnswer")),
                true
            ));
        }
        
        while (await reader.ReadAsync())
        {
            choices.Add(new MultipleChoiceAnswerModel(
                reader.GetString(reader.GetOrdinal("OptionText")),
                false
            ));
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

    private async Task<FlashcardExercise> GetFlashcardExerciseAsync(int id, string question, Difficulty difficulty)
    {
        using var connection = await _databaseConnection.CreateConnectionAsync();
        using var command = connection.CreateCommand();
        
        command.CommandText = "sp_GetFlashcardExercise";
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@exerciseId", id);
        
        await connection.OpenAsync();
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var answer = reader.GetString(reader.GetOrdinal("Answer"));
            var timeInSeconds = reader.GetInt32(reader.GetOrdinal("TimeInSeconds"));
            
            return new FlashcardExercise(id, question, answer, timeInSeconds, difficulty);
        }
        
        throw new KeyNotFoundException($"Flashcard exercise with ID {id} not found.");
    }
}
