CREATE OR ALTER PROCEDURE sp_UpdateSection
    @sectionId INT,
    @subjectId INT,
    @title VARCHAR(255),
    @description VARCHAR(500),
    @roadmapId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

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
            AND Id != @sectionId
        )
        BEGIN
            RAISERROR ('Order number already exists in this roadmap', 16, 1) WITH NOWAIT;
        END

        -- Update the section
        UPDATE Sections
        SET 
            SubjectId = @subjectId,
            Title = @title,
            Description = @description,
            RoadmapId = @roadmapId,
            OrderNumber = @orderNumber
        WHERE Id = @sectionId;
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