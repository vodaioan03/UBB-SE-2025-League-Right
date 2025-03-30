CREATE OR ALTER PROCEDURE sp_AddExerciseToQuiz
    @quizId INT,
    @exerciseId INT,
    @newId INT OUTPUT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        THROW 50001, 'Quiz not found.', 1;
    END

    -- Check if exercise exists
    IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
    BEGIN
        THROW 50002, 'Exercise not found.', 1;
    END

    INSERT INTO QuizExercises (QuizId, ExerciseId)
    VALUES (@quizId, @exerciseId);

    SET @newId = SCOPE_IDENTITY();
END; 