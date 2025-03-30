CREATE OR ALTER PROCEDURE sp_DeleteAssociationExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM AssociationExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            THROW 50001, 'Association exercise not found', 1;
        END

        -- Delete the association exercise (this will cascade delete the pairs due to foreign key)
        DELETE FROM AssociationExercises
        WHERE ExerciseId = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 