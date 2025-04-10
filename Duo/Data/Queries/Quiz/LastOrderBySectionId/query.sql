CREATE OR ALTER PROCEDURE sp_LastOrderQuizBySectionId
    @sectionId INT
AS
BEGIN
    SELECT ISNULL(MAX(OrderNumber), 0) AS LastOrderNumber FROM Quizzes
    WHERE SectionId = @sectionId;
END;