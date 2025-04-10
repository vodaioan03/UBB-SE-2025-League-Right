CREATE OR ALTER PROCEDURE sp_AddExerciseToExam
    @examId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if exam exists
    IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
    BEGIN
        RAISERROR ('Exam not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists
    IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
    BEGIN
        RAISERROR ( 'Exercise not found.', 16, 1) WITH NOWAIT;
    END

    INSERT INTO ExamExercises (ExamId, ExerciseId)
    VALUES (@examId, @exerciseId);

END; 