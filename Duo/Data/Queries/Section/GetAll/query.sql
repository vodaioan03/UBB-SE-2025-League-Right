CREATE OR ALTER PROCEDURE sp_GetAllSections
AS
BEGIN
    SELECT * FROM Sections
    ORDER BY RoadmapId, OrderNumber;
END; 