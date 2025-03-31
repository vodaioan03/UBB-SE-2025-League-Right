CREATE OR ALTER PROCEDURE sp_AddFillInTheBlanksExercise
    @exerciseId INT,
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
            RAISEERROR('Invalid exercise ID or type', 16, 1) WITH NOWAIT;
        END

        -- Insert the fill in the blanks exercise
        INSERT INTO FillInTheBlanksExercises (ExerciseId)
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