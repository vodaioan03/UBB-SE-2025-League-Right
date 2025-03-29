CREATE OR ALTER PROCEDURE sp_AddAssociationExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'Association'
        )
        BEGIN
            THROW 50001, 'Invalid exercise ID or type', 1;
        END

        -- Insert the association exercise
        INSERT INTO AssociationExercises (ExerciseId)
        VALUES (@exerciseId);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 