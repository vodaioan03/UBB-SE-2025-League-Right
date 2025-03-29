CREATE OR ALTER PROCEDURE sp_DeleteFillInTheBlanksAnswer
    @answerId INT
AS
BEGIN
    BEGIN TRY
        -- Check if answer exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FillInTheBlanksAnswers 
            WHERE Id = @answerId
        )
        BEGIN
            THROW 50001, 'Fill in the Blanks answer not found', 1;
        END

        -- Delete the answer
        DELETE FROM FillInTheBlanksAnswers
        WHERE Id = @answerId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 