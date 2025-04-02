CREATE OR ALTER PROCEDURE sp_UpdateUserProgress
    @Id INT,
    @LastCompletedSectionId INT = NULL,
    @LastCompletedQuizId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET
        NumberOfCompletedSections = COALESCE(@LastCompletedSectionId, NumberOfCompletedSections),
        NumberOfCompletedQuizzesInSection = COALESCE(@LastCompletedQuizId, NumberOfCompletedQuizzesInSection)
    WHERE Id = @Id;
END;