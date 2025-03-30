CREATE OR ALTER PROCEDURE sp_GetUnassignedExams
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId IS NULL;
END; 