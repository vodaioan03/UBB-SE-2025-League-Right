CREATE OR ALTER PROCEDURE sp_GetQuizById
    @quizId INT
AS
BEGIN
    SELECT q.*, qe.OrderNumber as ExerciseOrder, e.*, d.Name as DifficultyName
    FROM Quizzes q
    LEFT JOIN QuizExercises qe ON q.Id = qe.QuizId
    LEFT JOIN Exercises e ON qe.ExerciseId = e.Id
    LEFT JOIN Difficulties d ON e.DifficultyId = d.Id
    WHERE q.Id = @quizId
    ORDER BY qe.OrderNumber;
END; 