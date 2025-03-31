CREATE OR ALTER PROCEDURE sp_DeleteMultipleChoiceExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM MultipleChoiceExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISERROR('Multiple Choice exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise (this will cascade delete the options due to foreign key)
        DELETE FROM MultipleChoiceExercises
        WHERE ExerciseId = @exerciseId;
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