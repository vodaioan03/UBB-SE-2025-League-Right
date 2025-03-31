CREATE OR ALTER PROCEDURE sp_AddRoadmap
    @name VARCHAR(100),
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if name already exists
        IF EXISTS (SELECT 1 FROM Roadmaps WHERE Name = @name)
        BEGIN
            RAISERROR ('Roadmap with this name already exists', 16, 1) WITH NOWAIT;
        END

        -- Insert the new roadmap
        INSERT INTO Roadmaps (Name)
        VALUES (@name);

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