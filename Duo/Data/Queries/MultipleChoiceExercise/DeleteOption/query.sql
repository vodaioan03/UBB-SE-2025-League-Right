CREATE OR ALTER PROCEDURE sp_DeleteMultipleChoiceOption
    @optionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if option exists
        IF NOT EXISTS (
            SELECT 1 
            FROM MultipleChoiceOptions 
            WHERE Id = @optionId
        )
        BEGIN
            RAISERROR('Option not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the option
        DELETE FROM MultipleChoiceOptions
        WHERE Id = @optionId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END; 