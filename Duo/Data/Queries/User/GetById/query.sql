CREATE OR ALTER PROCEDURE sp_GetUserById
    @userId INT
AS
BEGIN
    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @userId)
        BEGIN
            RAISERROR ('User not found', 16, 1) WITH NOWAIT;
        END

        -- Get user's last completed section and quiz
        SELECT * FROM Users u
        WHERE u.Id = @userId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;