CREATE OR ALTER PROCEDURE sp_GetQuizzesBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId = @sectionId
    ORDER BY OrderNumber;
END; 