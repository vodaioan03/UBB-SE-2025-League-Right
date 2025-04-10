CREATE OR ALTER PROCEDURE sp_AddMultipleChoiceOption
    @exerciseId INT,
    @optionText VARCHAR(500)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises e
            JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISERROR('Invalid exercise ID or not a Multiple Choice exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the option
        INSERT INTO MultipleChoiceOptions (ExerciseId, OptionText)
        VALUES (@exerciseId, @optionText);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END; 