CREATE OR ALTER PROCEDURE sp_LastOrderSectionByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT ISNULL(MAX(OrderNumber), 0) AS LastOrderNumber FROM Sections
    WHERE RoadmapId = @roadmapId;
END;