CREATE OR ALTER PROCEDURE sp_GetFillInTheBlankExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT fite.*, fita.CorrectAnswer
    FROM FillInTheBlankExercises fite
    LEFT JOIN FillInTheBlankAnswers fita ON fite.ExerciseId = fita.ExerciseId
    WHERE fite.ExerciseId = @exerciseId
END; 