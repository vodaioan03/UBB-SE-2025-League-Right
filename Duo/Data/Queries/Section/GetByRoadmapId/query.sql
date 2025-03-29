CREATE OR ALTER PROCEDURE sp_GetSectionsByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Sections
    WHERE RoadmapId = @roadmapId
    ORDER BY OrderNumber;
END; 