CREATE OR ALTER PROCEDURE sp_DeleteQuiz
    @quizId INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            THROW 50001, 'Quiz not found', 1;
        END

        -- Delete quiz exercises first
        DELETE FROM QuizExercises
        WHERE QuizId = @quizId;

        -- Delete the quiz
        DELETE FROM Quizzes
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 