CREATE OR ALTER PROCEDURE sp_GetExamById
    @examId INT
AS
BEGIN
    SELECT ex.*
    FROM Exams ex
    WHERE ex.Id = @examId;
END; 