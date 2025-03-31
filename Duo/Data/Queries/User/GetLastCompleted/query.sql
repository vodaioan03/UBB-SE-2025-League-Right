CREATE OR ALTER PROCEDURE sp_GetUserLastCompleted
    @userId INT
AS
BEGIN
    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @userId)
        BEGIN
            THROW 50001, 'User not found', 1;
        END

        -- Get user's last completed section and quiz
        SELECT 
            u.Id AS UserId,
            u.Username,
            u.LastCompletedSectionId,
            u.LastCompletedQuizId
        FROM Users u
        WHERE u.Id = @userId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 