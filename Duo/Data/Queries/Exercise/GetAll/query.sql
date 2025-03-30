CREATE OR ALTER PROCEDURE sp_GetAllExercises
AS
BEGIN
    SELECT 
        e.*,
        d.Name as DifficultyName,
        -- Multiple Choice Exercise data
        mce.Question as MultipleChoiceQuestion,
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blanks Exercise data
        fbe.Sentence as FillInTheBlanksSentence,
        fba.CorrectAnswer as FillInTheBlanksAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Sentence as FlashcardSentence,
        fe.Answer as FlashcardAnswer,
        fe.TimeInSeconds as FlashcardTimeInSeconds
    FROM Exercises e
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
END; 