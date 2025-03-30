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
            THROW 50001, 'Multiple Choice option not found', 1;
        END

        -- Delete the option
        DELETE FROM MultipleChoiceOptions
        WHERE Id = @optionId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 