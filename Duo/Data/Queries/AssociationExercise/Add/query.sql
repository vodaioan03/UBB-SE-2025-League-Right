CREATE OR ALTER PROCEDURE sp_AddAssociationExercise
    @exerciseId INT,
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'AssociationExercise'
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or type.', 16, 1) WITH NOWAIT;
        END

        -- Insert the association exercise
        INSERT INTO AssociationExercises (ExerciseId)
        VALUES (@exerciseId);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 