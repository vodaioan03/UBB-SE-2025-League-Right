CREATE OR ALTER PROCEDURE sp_AddExerciseToExam
    @examId INT,
    @exerciseId INT,
    @newId INT OUTPUT
AS
BEGIN
    -- Check if exam exists
    IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
    BEGIN
        THROW 50001, 'Exam not found.', 1;
    END

    -- Check if exercise exists
    IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
    BEGIN
        THROW 50002, 'Exercise not found.', 1;
    END

    INSERT INTO ExamExercises (ExamId, ExerciseId)
    VALUES (@examId, @exerciseId);

    SET @newId = SCOPE_IDENTITY();
END; 