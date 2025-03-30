CREATE OR ALTER PROCEDURE sp_DeleteExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
        BEGIN
            THROW 50001, 'Exercise not found', 1;
        END

        -- Delete the base exercise - cascading will handle the rest
        DELETE FROM Exercises WHERE Id = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 