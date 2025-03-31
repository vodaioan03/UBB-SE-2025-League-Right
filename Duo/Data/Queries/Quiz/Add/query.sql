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
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            RAISERROR ('Order number already exists in this section', 16, 1) WITH NOWAIT;
        END

        -- Insert the new quiz
        INSERT INTO Quizzes (SectionId, OrderNumber)
        VALUES (@sectionId, @orderNumber);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
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