CREATE PROCEDURE sp_UpdateUserProgress
    @Id INT,
    @LastCompletedSectionId INT,
    @LastCompletedQuizId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users
    SET 
        LastCompletedSectionId = @LastCompletedSectionId,
        LastCompletedQuizId = @LastCompletedQuizId
    WHERE Id = @Id;
END
