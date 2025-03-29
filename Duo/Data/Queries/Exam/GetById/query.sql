CREATE OR ALTER PROCEDURE sp_GetExamById
    @examId INT
AS
BEGIN
    SELECT ex.*, ee.OrderNumber as ExerciseOrder, e.*, d.Name as DifficultyName
    FROM Exams ex
    LEFT JOIN ExamExercises ee ON ex.Id = ee.ExamId
    LEFT JOIN Exercises e ON ee.ExerciseId = e.Id
    LEFT JOIN Difficulties d ON e.DifficultyId = d.Id
    WHERE ex.Id = @examId
    ORDER BY ee.OrderNumber;
END; 