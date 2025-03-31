CREATE OR ALTER PROCEDURE sp_GetExamByIdWithExercises
    @examId INT
AS
BEGIN
    SELECT 
        ex.*,
        e.*,
        d.Name as DifficultyName,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blanks Exercise data
        fba.CorrectAnswer as FillInTheBlanksAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Exams ex
    LEFT JOIN ExamExercises ee ON ex.Id = ee.ExamId
    LEFT JOIN Exercises e ON ee.ExerciseId = e.Id
    LEFT JOIN Difficulties d ON e.DifficultyId = d.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blanks Exercise joins
    LEFT JOIN FillInTheBlanksExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlanksAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE ex.Id = @examId;
END; 