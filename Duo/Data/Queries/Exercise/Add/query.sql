CREATE OR ALTER PROCEDURE sp_AddExercise
    @type VARCHAR(50),
    @difficultyId INT,
    -- Multiple Choice parameters
    @question VARCHAR(500) = NULL,
    @correctAnswer VARCHAR(500) = NULL,
    -- Fill in the Blanks parameters
    @sentence VARCHAR(500) = NULL,
    -- Output parameter
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Validate exercise type
        IF @type NOT IN ('MultipleChoice', 'FillInTheBlanks', 'Association')
        BEGIN
            THROW 50001, 'Invalid exercise type', 1;
        END

        -- Check if difficulty exists
        IF NOT EXISTS (SELECT 1 FROM Difficulties WHERE Id = @difficultyId)
        BEGIN
            THROW 50002, 'Difficulty not found', 1;
        END

        -- Insert the base exercise
        INSERT INTO Exercises (Type, DifficultyId)
        VALUES (@type, @difficultyId);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();

        -- Handle specific exercise type creation
        IF @type = 'MultipleChoice'
        BEGIN
            -- Validate required parameters
            IF @question IS NULL OR @correctAnswer IS NULL
            BEGIN
                THROW 50003, 'Question and correct answer are required for MultipleChoice exercises', 1;
            END

            -- Insert the multiple choice exercise
            INSERT INTO MultipleChoiceExercises (ExerciseId, Question, CorrectAnswer)
            VALUES (@newId, @question, @correctAnswer);
        END
        ELSE IF @type = 'FillInTheBlanks'
        BEGIN
            -- Validate required parameters
            IF @sentence IS NULL
            BEGIN
                THROW 50004, 'Sentence is required for FillInTheBlanks exercises', 1;
            END

            -- Insert the fill in the blanks exercise
            INSERT INTO FillInTheBlanksExercises (ExerciseId, Sentence)
            VALUES (@newId, @sentence);
        END
        ELSE IF @type = 'Association'
        BEGIN
            -- Insert the association exercise (no additional data needed)
            INSERT INTO AssociationExercises (ExerciseId)
            VALUES (@newId);
        END
    END TRY
    BEGIN CATCH
        -- If an error occurred, rollback the base exercise creation
        IF @newId IS NOT NULL
        BEGIN
            DELETE FROM Exercises WHERE Id = @newId;
        END
        THROW;
    END CATCH
END; 