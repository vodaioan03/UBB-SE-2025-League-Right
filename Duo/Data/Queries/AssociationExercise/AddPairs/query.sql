CREATE OR ALTER PROCEDURE sp_AddAssociationPair
    @exerciseId INT,
    @firstAnswer VARCHAR(255),
    @secondAnswer VARCHAR(255)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises e
            JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or not an Association exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the pair
        INSERT INTO AssociationPairs (ExerciseId, FirstAnswer, SecondAnswer)
        VALUES (@exerciseId, @firstAnswer, @secondAnswer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 