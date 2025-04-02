CREATE OR ALTER PROCEDURE sp_CountQuizzesBySectionId
    @sectionId INT
AS
BEGIN
    SELECT COUNT(*) AS QuizCount FROM Quizzes
    WHERE SectionId = @sectionId;
END;