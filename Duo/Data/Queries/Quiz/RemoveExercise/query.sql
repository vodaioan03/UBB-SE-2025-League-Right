CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromQuiz
    @quizId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        THROW 50001, 'Quiz not found.', 1;
    END

    -- Check if exercise exists in quiz
    IF NOT EXISTS (SELECT 1 FROM QuizExercises WHERE QuizId = @quizId AND ExerciseId = @exerciseId)
    BEGIN
        THROW 50002, 'Exercise not found in this quiz.', 1;
    END

    DELETE FROM QuizExercises
    WHERE QuizId = @quizId AND ExerciseId = @exerciseId;
END; 