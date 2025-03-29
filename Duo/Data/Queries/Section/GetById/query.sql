CREATE OR ALTER PROCEDURE sp_GetSectionById
    @sectionId INT
AS
BEGIN
    SELECT * FROM Sections
    WHERE Id = @sectionId;
END; 