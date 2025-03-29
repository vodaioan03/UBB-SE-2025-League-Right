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
            THROW 50001, 'Multiple Choice exercise not found', 1;
        END

        -- Delete the exercise (this will cascade delete the options due to foreign key)
        DELETE FROM MultipleChoiceExercises
        WHERE ExerciseId = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 