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
            RAISEERROR('Invalid exercise ID or not a Fill in the Blanks exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the answer
        INSERT INTO FillInTheBlanksAnswers (ExerciseId, CorrectAnswer)
        VALUES (@exerciseId, @correctAnswer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 