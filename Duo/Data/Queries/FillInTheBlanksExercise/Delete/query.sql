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
            THROW 50001, 'Fill in the Blanks exercise not found', 1;
        END

        -- Delete the exercise (this will cascade delete the answers due to foreign key)
        DELETE FROM FillInTheBlanksExercises
        WHERE ExerciseId = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 