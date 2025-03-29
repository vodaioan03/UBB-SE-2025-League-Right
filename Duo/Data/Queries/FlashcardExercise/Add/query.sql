CREATE OR ALTER PROCEDURE sp_AddFlashcardExercise
    @exerciseId INT,
    @sentence VARCHAR(500),
    @answer VARCHAR(100),
    @timeInSeconds INT
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
            THROW 50001, 'Invalid exercise ID or type', 1;
        END

        -- Insert the flashcard exercise
        INSERT INTO FlashcardExercises (ExerciseId, Sentence, Answer, TimeInSeconds)
        VALUES (@exerciseId, @sentence, @answer, @timeInSeconds);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 