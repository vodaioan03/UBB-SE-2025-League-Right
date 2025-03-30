CREATE OR ALTER PROCEDURE sp_DeleteSection
    @sectionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            THROW 50001, 'Section not found', 1;
        END

        -- Delete all quizzes in this section
        DELETE FROM QuizExercises
        WHERE QuizId IN (SELECT Id FROM Quizzes WHERE SectionId = @sectionId);
        DELETE FROM Quizzes WHERE SectionId = @sectionId;

        -- Delete all exams in this section
        DELETE FROM ExamExercises
        WHERE ExamId IN (SELECT Id FROM Exams WHERE SectionId = @sectionId);
        DELETE FROM Exams WHERE SectionId = @sectionId;

        -- Update any users who had this as their last completed section
        UPDATE Users
        SET LastCompletedSectionId = NULL
        WHERE LastCompletedSectionId = @sectionId;

        -- Finally delete the section
        DELETE FROM Sections
        WHERE Id = @sectionId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 