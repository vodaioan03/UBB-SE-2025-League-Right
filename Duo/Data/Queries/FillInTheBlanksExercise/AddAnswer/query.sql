CREATE OR ALTER PROCEDURE sp_AddFillInTheBlanksAnswer
    @exerciseId INT,
    @correctAnswer VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises e
            JOIN FillInTheBlanksExercises fbe ON e.Id = fbe.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            THROW 50001, 'Invalid exercise ID or not a Fill in the Blanks exercise', 1;
        END

        -- Insert the answer
        INSERT INTO FillInTheBlanksAnswers (ExerciseId, CorrectAnswer)
        VALUES (@exerciseId, @correctAnswer);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 