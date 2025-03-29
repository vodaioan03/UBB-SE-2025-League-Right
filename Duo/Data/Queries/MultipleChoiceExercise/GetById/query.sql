CREATE OR ALTER PROCEDURE sp_GetMultipleChoiceExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT mce.*, mco.OptionText
    FROM MultipleChoiceExercises mce
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    WHERE mce.ExerciseId = @exerciseId;
END; 