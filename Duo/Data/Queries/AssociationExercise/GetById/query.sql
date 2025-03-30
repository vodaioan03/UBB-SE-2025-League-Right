CREATE OR ALTER PROCEDURE sp_GetAssociationExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT ae.*, ap.FirstAnswer, ap.SecondAnswer
    FROM AssociationExercises ae
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    WHERE ae.ExerciseId = @exerciseId;
END; 