CREATE PROCEDURE sp_GetUserByUsername
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Username,
        LastCompletedSectionId,
        LastCompletedQuizId
    FROM Users
    WHERE Username = @Username;
END
