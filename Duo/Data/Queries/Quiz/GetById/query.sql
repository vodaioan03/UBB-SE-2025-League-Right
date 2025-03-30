CREATE OR ALTER PROCEDURE sp_GetQuizById
    @quizId INT
AS
BEGIN
    SELECT q.*
    FROM Quizzes q
    WHERE q.Id = @quizId;
END; 