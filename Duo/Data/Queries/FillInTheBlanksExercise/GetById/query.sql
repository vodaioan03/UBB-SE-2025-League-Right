CREATE OR ALTER PROCEDURE sp_GetFillInTheBlanksExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT fite.*, fita.CorrectAnswer
    FROM FillInTheBlanksExercises fite
    LEFT JOIN FillInTheBlanksAnswers fita ON fite.ExerciseId = fita.ExerciseId
    WHERE fite.ExerciseId = @exerciseId
END; 