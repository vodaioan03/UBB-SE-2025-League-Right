CREATE OR ALTER PROCEDURE sp_GetAllQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    ORDER BY SectionId, OrderNumber;
END; 