CREATE OR ALTER PROCEDURE sp_DeleteRoadmap
    @roadmapId INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            THROW 50001, 'Roadmap not found', 1;
        END

        DELETE FROM Roadmaps
        WHERE Id = @roadmapId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 