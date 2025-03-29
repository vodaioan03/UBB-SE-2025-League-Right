CREATE OR ALTER PROCEDURE sp_AddMultipleChoiceExercise
    @exerciseId INT,
    @question VARCHAR(500),
    @correctAnswer VARCHAR(500)
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'MultipleChoice'
        )
        BEGIN
            THROW 50001, 'Invalid exercise ID or type', 1;
        END

        -- Insert the multiple choice exercise
        INSERT INTO MultipleChoiceExercises (ExerciseId, Question, CorrectAnswer)
        VALUES (@exerciseId, @question, @correctAnswer);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 