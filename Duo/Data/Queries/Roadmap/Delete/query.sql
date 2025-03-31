CREATE OR ALTER PROCEDURE sp_DeleteRoadmap
    @roadmapId INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            RAISERROR ('Roadmap not found', 16, 1) WITH NOWAIT;
        END

        DELETE FROM Roadmaps
        WHERE Id = @roadmapId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 