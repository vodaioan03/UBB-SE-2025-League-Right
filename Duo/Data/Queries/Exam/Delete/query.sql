CREATE OR ALTER PROCEDURE sp_DeleteExam
    @examId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            THROW 50001, 'Exam not found', 1;
        END

        -- Delete exam exercises first
        DELETE FROM ExamExercises
        WHERE ExamId = @examId;

        -- Delete the exam
        DELETE FROM Exams
        WHERE Id = @examId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 