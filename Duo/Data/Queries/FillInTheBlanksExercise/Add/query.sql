CREATE OR ALTER PROCEDURE sp_AddFillInTheBlanksExercise
    @exerciseId INT,
    @sentence VARCHAR(500)
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'FillInTheBlanks'
        )
        BEGIN
            THROW 50001, 'Invalid exercise ID or type', 1;
        END

        -- Insert the fill in the blanks exercise
        INSERT INTO FillInTheBlanksExercises (ExerciseId, Sentence)
        VALUES (@exerciseId, @sentence);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 