CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromExam
    @examId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if exam exists
    IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
    BEGIN
        RAISERROR ('Exam not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists in exam
    IF NOT EXISTS (SELECT 1 FROM ExamExercises WHERE ExamId = @examId AND ExerciseId = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found in this exam.', 16, 1) WITH NOWAIT;
    END

    DELETE FROM ExamExercises
    WHERE ExamId = @examId AND ExerciseId = @exerciseId;
END; 