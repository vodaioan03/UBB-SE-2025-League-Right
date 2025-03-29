CREATE OR ALTER PROCEDURE sp_AddRoadmap
    @name VARCHAR(100),
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if name already exists
        IF EXISTS (SELECT 1 FROM Roadmaps WHERE Name = @name)
        BEGIN
            THROW 50001, 'Roadmap with this name already exists', 1;
        END

        -- Insert the new roadmap
        INSERT INTO Roadmaps (Name)
        VALUES (@name);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 