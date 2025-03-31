CREATE OR ALTER PROCEDURE sp_AddExerciseToQuiz
    @quizId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        RAISERROR ('Quiz not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists
    IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found.', 16, 1) WITH NOWAIT;
    END

    INSERT INTO QuizExercises (QuizId, ExerciseId)
    VALUES (@quizId, @exerciseId);

END;