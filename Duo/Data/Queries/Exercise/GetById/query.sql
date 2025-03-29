CREATE OR ALTER PROCEDURE sp_GetExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT e.*, d.Name as DifficultyName, d.Description as DifficultyDescription
    FROM Exercises e
    JOIN Difficulties d ON e.DifficultyId = d.Id
    WHERE e.Id = @exerciseId;
END; 