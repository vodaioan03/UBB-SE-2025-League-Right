CREATE OR ALTER PROCEDURE sp_AddSection
    @subjectId INT,
    @title VARCHAR(255),
    @description TEXT,
    @roadmapId INT,
    @orderNumber INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if roadmap exists
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            RAISERROR ('Roadmap not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the roadmap
        IF EXISTS (
            SELECT 1 
            FROM Sections 
            WHERE RoadmapId = @roadmapId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            RAISERROR ('Order number already exists in this roadmap', 16 ,1) WITH NOWAIT;
        END

        -- Insert the new section
        INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber)
        VALUES (@subjectId, @title, @description, @roadmapId, @orderNumber);

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