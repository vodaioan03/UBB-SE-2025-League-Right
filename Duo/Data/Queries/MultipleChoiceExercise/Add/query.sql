CREATE OR ALTER PROCEDURE sp_AddMultipleChoiceExercise
    @exerciseId INT,
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
            RAISERROR('Exercise not found or not of type MultipleChoice', 16, 1) WITH NOWAIT;
        END

        -- Insert the multiple choice exercise
        INSERT INTO MultipleChoiceExercises (ExerciseId, CorrectAnswer)
        VALUES (@exerciseId, @correctAnswer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END; 