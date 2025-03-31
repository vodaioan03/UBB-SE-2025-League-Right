CREATE OR ALTER PROCEDURE sp_DeleteFlashcardExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FlashcardExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISERROR('Flashcard exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise
        DELETE FROM FlashcardExercises
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