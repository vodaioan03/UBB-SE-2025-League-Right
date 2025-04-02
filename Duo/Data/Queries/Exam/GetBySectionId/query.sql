CREATE OR ALTER PROCEDURE sp_GetExamBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId = @sectionId
END; 