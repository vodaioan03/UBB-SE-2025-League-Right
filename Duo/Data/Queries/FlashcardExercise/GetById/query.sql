CREATE OR ALTER PROCEDURE sp_GetFlashcardExercise
    @exerciseId INT
AS
BEGIN
    SELECT 
        fe.*
    FROM FlashcardExercises fe
    WHERE fe.ExerciseId = @exerciseId;
END; 