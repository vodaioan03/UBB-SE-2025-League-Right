CREATE OR ALTER PROCEDURE sp_AddExam
    @sectionId INT,
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50001, 'Section not found', 1;
        END

        -- Insert the new exam
        INSERT INTO Exams (SectionId)
        VALUES (@sectionId);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 