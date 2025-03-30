CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromExam
    @examId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if exam exists
    IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
    BEGIN
        THROW 50001, 'Exam not found.', 1;
    END

    -- Check if exercise exists in exam
    IF NOT EXISTS (SELECT 1 FROM ExamExercises WHERE ExamId = @examId AND ExerciseId = @exerciseId)
    BEGIN
        THROW 50002, 'Exercise not found in this exam.', 1;
    END

    DELETE FROM ExamExercises
    WHERE ExamId = @examId AND ExerciseId = @exerciseId;
END; 