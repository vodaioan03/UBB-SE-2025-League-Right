CREATE OR ALTER PROCEDURE sp_DeleteFillInTheBlankAnswer
    @answerId INT
AS
BEGIN
    BEGIN TRY
        -- Check if answer exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FillInTheBlankAnswers 
            WHERE Id = @answerId
        )
        BEGIN
            RAISERROR ('Fill in the Blank answer not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the answer
        DELETE FROM FillInTheBlankAnswers
        WHERE Id = @answerId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 