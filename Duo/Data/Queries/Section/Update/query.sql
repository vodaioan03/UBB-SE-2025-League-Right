CREATE OR ALTER PROCEDURE sp_UpdateSection
    @sectionId INT,
    @subjectId INT,
    @title VARCHAR(255),
    @description TEXT,
    @roadmapId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50001, 'Section not found', 1;
        END

        -- Check if roadmap exists
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            THROW 50002, 'Roadmap not found', 1;
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
            THROW 50003, 'Order number already exists in this roadmap', 1;
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
        THROW;
    END CATCH
END; 