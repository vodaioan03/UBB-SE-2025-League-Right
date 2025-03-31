CREATE OR ALTER PROCEDURE sp_AddExercise
    @type VARCHAR(50),
    @difficultyId INT,
    @question VARCHAR(500),
    -- Multiple Choice parameters
    @correctAnswer VARCHAR(500) = NULL,
    -- Fill in the Blanks parameters
    @sentence VARCHAR(500) = NULL,
    -- Association parameters
    @associationSentence VARCHAR(500) = NULL,
    -- Flashcard parameters
    @flashcardAnswer VARCHAR(100) = NULL,
    -- Output parameter
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Validate exercise type
        IF @type NOT IN ('MultipleChoiceExercise', 'FillInTheBlanksExercise', 'AssociationExercise', 'FlashcardExercise')
        BEGIN
            RAISERROR ('Invalid exercise type', 16, 1) WITH NOWAIT;
        END

        -- Check if difficulty exists
        IF NOT EXISTS (SELECT 1 FROM Difficulties WHERE Id = @difficultyId)
        BEGIN
            RAISERROR ('Difficulty not found', 16, 1) WITH NOWAIT;
        END

        -- Validate question is not null
        IF @question IS NULL
        BEGIN
            RAISERROR ('Question is required for all exercises', 16, 1) WITH NOWAIT;
        END

        -- Insert the base exercise
        INSERT INTO Exercises (Type, DifficultyId, Question)
        VALUES (@type, @difficultyId, @question);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();

        -- Handle specific exercise type creation
        IF @type = 'MultipleChoiceExercise'
        BEGIN
            -- Validate required parameters
            IF @correctAnswer IS NULL
            BEGIN
                RAISERROR ('Correct answer is required for MultipleChoice exercises', 16, 1) WITH NOWAIT;
            END

            -- Insert the multiple choice exercise
            INSERT INTO MultipleChoiceExercises (ExerciseId, CorrectAnswer)
            VALUES (@newId, @correctAnswer);
        END
        ELSE IF @type = 'FillInTheBlanksExercise'
        BEGIN
            -- Insert the fill in the blanks exercise
            INSERT INTO FillInTheBlanksExercises (ExerciseId)
            VALUES (@newId);
        END
        ELSE IF @type = 'AssociationExercise'
        BEGIN
            -- Insert the association exercise
            INSERT INTO AssociationExercises (ExerciseId)
            VALUES (@newId);
        END
        ELSE IF @type = 'FlashcardExercise'
        BEGIN
            -- Validate required parameters
            IF @flashcardAnswer IS NULL
            BEGIN
                RAISERROR ('Answer is required for Flashcard exercises', 16, 1) WITH NOWAIT;
            END

            -- Insert the flashcard exercise
            INSERT INTO FlashcardExercises (ExerciseId, Answer)
            VALUES (@newId, @flashcardAnswer);
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        -- If an error occurred, rollback the base exercise creation
        IF @newId IS NOT NULL
        BEGIN
            DELETE FROM Exercises WHERE Id = @newId;
        END
        RAISERROR (@ErrorMessage, 16, 1) WITH NOWAIT;
    END CATCH
END; 