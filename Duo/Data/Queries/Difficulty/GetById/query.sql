CREATE OR ALTER PROCEDURE sp_GetDifficultyById
    @difficultyId INT
AS
BEGIN
    SELECT * FROM Difficulties
    WHERE Id = @difficultyId;
END; 