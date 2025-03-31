CREATE OR ALTER PROCEDURE sp_DeleteFillInTheBlanksExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FillInTheBlanksExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISEERROR('Fill in the Blanks exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise (this will cascade delete the answers due to foreign key)
        DELETE FROM FillInTheBlanksExercises
        WHERE ExerciseId = @exerciseId;
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