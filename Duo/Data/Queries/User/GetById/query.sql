CREATE PROCEDURE sp_GetUserById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Username,
        LastCompletedSectionId,
        LastCompletedQuizId
    FROM Users
    WHERE Id = @Id;
END
