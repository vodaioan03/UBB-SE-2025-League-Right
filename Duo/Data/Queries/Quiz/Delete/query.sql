CREATE OR ALTER PROCEDURE sp_DeleteQuiz
    @quizId INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the quiz
        DELETE FROM Quizzes
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;