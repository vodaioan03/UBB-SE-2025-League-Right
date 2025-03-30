CREATE OR ALTER PROCEDURE sp_GetUnassignedQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId IS NULL;
END; 