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
            THROW 50001, 'Invalid exercise ID or not an Association exercise', 1;
        END

        -- Insert the pair
        INSERT INTO AssociationPairs (ExerciseId, FirstAnswer, SecondAnswer)
        VALUES (@exerciseId, @firstAnswer, @secondAnswer);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 