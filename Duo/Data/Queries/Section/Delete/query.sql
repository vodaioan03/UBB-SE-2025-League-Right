CREATE OR ALTER PROCEDURE sp_DeleteSection
    @sectionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Delete all quizzes in this section
        DELETE FROM QuizExercises
        WHERE QuizId IN (SELECT Id FROM Quizzes WHERE SectionId = @sectionId);
        DELETE FROM Quizzes WHERE SectionId = @sectionId;

        -- Delete all exams in this section
        DELETE FROM ExamExercises
        WHERE ExamId IN (SELECT Id FROM Exams WHERE SectionId = @sectionId);
        DELETE FROM Exams WHERE SectionId = @sectionId;

        -- Finally delete the section
        DELETE FROM Sections
        WHERE Id = @sectionId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 