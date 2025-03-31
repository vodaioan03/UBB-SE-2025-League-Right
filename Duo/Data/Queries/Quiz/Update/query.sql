CREATE OR ALTER PROCEDURE sp_UpdateQuiz
    @quizId INT,
    @sectionId INT = NULL,
    @orderNumber INT = NULL
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the section
        IF @sectionId IS NOT NULL AND @orderNumber IS NOT NULL AND EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            RAISERROR ('Order number already exists in this section', 16, 1) WITH NOWAIT;
        END
        -- Update the quiz
        UPDATE Quizzes
        SET 
            SectionId = @sectionId,
            OrderNumber = @orderNumber
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 