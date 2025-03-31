CREATE OR ALTER PROCEDURE sp_AddFlashcardExercise
    @exerciseId INT,
    @answer VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'Flashcard'
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or type', 16, 1) WITH NOWAIT;
        END

        -- Insert the flashcard exercise
        INSERT INTO FlashcardExercises (ExerciseId, Answer)
        VALUES (@exerciseId, @answer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 