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
            THROW 50001, 'Flashcard exercise not found', 1;
        END

        -- Delete the exercise
        DELETE FROM FlashcardExercises
        WHERE ExerciseId = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 