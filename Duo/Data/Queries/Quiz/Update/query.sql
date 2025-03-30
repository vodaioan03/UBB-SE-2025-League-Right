CREATE OR ALTER PROCEDURE sp_UpdateQuiz
    @quizId INT,
    @sectionId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            THROW 50001, 'Quiz not found', 1;
        END

        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50002, 'Section not found', 1;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber 
            AND Id != @quizId
        )
        BEGIN
            THROW 50003, 'Order number already exists in this section', 1;
        END

        -- Update the quiz
        UPDATE Quizzes
        SET 
            SectionId = @sectionId,
            OrderNumber = @orderNumber
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 