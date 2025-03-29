CREATE OR ALTER PROCEDURE sp_AddQuiz
    @sectionId INT,
    @orderNumber INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50001, 'Section not found', 1;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            THROW 50002, 'Order number already exists in this section', 1;
        END

        -- Insert the new quiz
        INSERT INTO Quizzes (SectionId, OrderNumber)
        VALUES (@sectionId, @orderNumber);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 