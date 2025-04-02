CREATE OR ALTER PROCEDURE sp_CountSectionsByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT COUNT(*) AS SectionCount FROM Sections
    WHERE RoadmapId = @roadmapId;
END; 