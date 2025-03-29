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
            THROW 50001, 'Roadmap not found', 1;
        END

        -- Check if order number is unique within the roadmap
        IF EXISTS (
            SELECT 1 
            FROM Sections 
            WHERE RoadmapId = @roadmapId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            THROW 50002, 'Order number already exists in this roadmap', 1;
        END

        -- Insert the new section
        INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber)
        VALUES (@subjectId, @title, @description, @roadmapId, @orderNumber);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 