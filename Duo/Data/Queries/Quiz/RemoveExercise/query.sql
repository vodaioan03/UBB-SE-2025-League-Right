CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromQuiz
    @quizId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        RAISERROR ('Quiz not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists in quiz
    IF NOT EXISTS (SELECT 1 FROM QuizExercises WHERE QuizId = @quizId AND ExerciseId = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found in this quiz.', 16, 1) WITH NOWAIT;
    END

    DELETE FROM QuizExercises
    WHERE QuizId = @quizId AND ExerciseId = @exerciseId;
END; 