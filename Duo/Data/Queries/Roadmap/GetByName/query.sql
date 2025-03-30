CREATE OR ALTER PROCEDURE sp_GetRoadmapByName
    @roadmapName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Roadmaps r
    WHERE r.Name = @roadmapName;
END;