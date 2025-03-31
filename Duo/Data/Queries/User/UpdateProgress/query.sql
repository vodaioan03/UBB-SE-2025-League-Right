CREATE PROCEDURE sp_UpdateUserProgress
    @Id INT,
    @LastCompletedSectionId INT = NULL,
    @LastCompletedQuizId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users
    SET 
        LastCompletedSectionId = COALESCE(@LastCompletedSectionId, LastCompletedSectionId),
        LastCompletedQuizId = COALESCE(@LastCompletedQuizId, LastCompletedQuizId)
    WHERE Id = @Id;
END