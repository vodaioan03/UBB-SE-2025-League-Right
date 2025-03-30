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
            THROW 50001, 'Invalid exercise ID or not a Multiple Choice exercise', 1;
        END

        -- Insert the option
        INSERT INTO MultipleChoiceOptions (ExerciseId, OptionText)
        VALUES (@exerciseId, @optionText);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 