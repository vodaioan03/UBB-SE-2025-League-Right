CREATE OR ALTER PROCEDURE sp_DeleteExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
        BEGIN
            THROW 50001, 'Exercise not found', 1;
        END

        -- Get the exercise type
        DECLARE @exerciseType VARCHAR(50);
        SELECT @exerciseType = Type FROM Exercises WHERE Id = @exerciseId;

        -- Delete related records based on exercise type
        IF @exerciseType = 'MultipleChoice'
        BEGIN
            DELETE FROM MultipleChoiceOptions WHERE ExerciseId = @exerciseId;
            DELETE FROM MultipleChoiceExercises WHERE ExerciseId = @exerciseId;
        END
        ELSE IF @exerciseType = 'FillInTheBlanks'
        BEGIN
            DELETE FROM FillInTheBlanksAnswers WHERE ExerciseId = @exerciseId;
            DELETE FROM FillInTheBlanksExercises WHERE ExerciseId = @exerciseId;
        END
        ELSE IF @exerciseType = 'Association'
        BEGIN
            DELETE FROM AssociationPairs WHERE ExerciseId = @exerciseId;
            DELETE FROM AssociationExercises WHERE ExerciseId = @exerciseId;
        END

        -- Delete from QuizExercises and ExamExercises
        DELETE FROM QuizExercises WHERE ExerciseId = @exerciseId;
        DELETE FROM ExamExercises WHERE ExerciseId = @exerciseId;

        -- Finally delete the base exercise
        DELETE FROM Exercises WHERE Id = @exerciseId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 