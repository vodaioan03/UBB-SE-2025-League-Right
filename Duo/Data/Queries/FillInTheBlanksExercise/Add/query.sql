CREATE OR ALTER PROCEDURE sp_AddFillInTheBlankExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'FillInTheBlank'
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or type', 16, 1) WITH NOWAIT;
        END

        -- Insert the fill in the Blank exercise
        INSERT INTO FillInTheBlankExercises (ExerciseId)
        VALUES (@exerciseId);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 