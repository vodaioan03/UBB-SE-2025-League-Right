CREATE OR ALTER PROCEDURE sp_UpdateExam
    @examId INT,
    @sectionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            THROW 50001, 'Exam not found', 1;
        END

        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50002, 'Section not found', 1;
        END

        -- Update the exam
        UPDATE Exams
        SET SectionId = @sectionId
        WHERE Id = @examId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 