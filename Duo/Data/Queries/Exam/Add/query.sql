CREATE OR ALTER PROCEDURE sp_AddExam
    @sectionId INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50001, 'Section not found', 1;
        END

        -- Check if section already has an exam
        IF EXISTS (SELECT 1 FROM Exams WHERE SectionId = @sectionId)
        BEGIN
            THROW 50002, 'Section already has an exam', 1;
        END

        -- Insert the new exam
        INSERT INTO Exams (SectionId)
        VALUES (@sectionId);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 