CREATE OR ALTER PROCEDURE sp_GetRoadmapById
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Roadmaps r
    WHERE r.Id = @roadmapId;
END;
