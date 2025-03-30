CREATE PROCEDURE sp_UpdateUserProgress
    @UserId INT,
    @LastCompletedSectionId INT = NULL,
    @LastCompletedQuizId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @UserId)
    BEGIN
        THROW 50001, 'User not found', 1;
    END

    -- Update user progress
    UPDATE Users
    SET 
        LastCompletedSectionId = ISNULL(@LastCompletedSectionId, LastCompletedSectionId),
        LastCompletedQuizId = ISNULL(@LastCompletedQuizId, LastCompletedQuizId)
    WHERE Id = @UserId;
END 