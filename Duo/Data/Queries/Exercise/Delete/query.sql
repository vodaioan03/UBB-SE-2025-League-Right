CREATE OR ALTER PROCEDURE sp_DeleteExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
        BEGIN
            RAISERROR ('Exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the base exercise - cascading will handle the rest
        DELETE FROM Exercises WHERE Id = @exerciseId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1) WITH NOWAIT;
    END CATCH
END; 