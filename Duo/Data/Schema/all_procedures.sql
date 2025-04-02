CREATE OR ALTER PROCEDURE sp_AddAssociationPair
    @exerciseId INT,
    @firstAnswer VARCHAR(255),
    @secondAnswer VARCHAR(255)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1
            FROM Exercises e
            JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or not an Association exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the pair
        INSERT INTO AssociationPairs (ExerciseId, FirstAnswer, SecondAnswer)
        VALUES (@exerciseId, @firstAnswer, @secondAnswer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllDifficulties
AS
BEGIN
    SELECT * FROM Difficulties;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetDifficultyById
    @difficultyId INT
AS
BEGIN
    SELECT * FROM Difficulties
    WHERE Id = @difficultyId;
END;
GO

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
GO

CREATE OR ALTER PROCEDURE sp_DeleteExam
    @examId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            RAISERROR ('Exam not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exam
        DELETE FROM Exams
        WHERE Id = @examId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllExams
AS
BEGIN
    SELECT *
    FROM Exams
    ORDER BY SectionId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetExamById
    @examId INT
AS
BEGIN
    SELECT ex.*
    FROM Exams ex
    WHERE ex.Id = @examId;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddExam
    @sectionId INT = NULL,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section already has an exam
        IF @sectionId IS NOT NULL AND EXISTS (SELECT 1 FROM Exams WHERE SectionId = @sectionId)
        BEGIN
            RAISERROR ('Section already has an exam', 16, 1) WITH NOWAIT;
        END

        -- Insert the new exam
        INSERT INTO Exams (SectionId)
        VALUES (@sectionId);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetExamByIdWithExercises
    @examId INT
AS
BEGIN
    SELECT
        ex.*,
        e.Id as ExerciseId,
        e.Type as Type,
        e.Question as Question,
        e.DifficultyId as DifficultyId,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blank Exercise data
        fba.CorrectAnswer as FillInTheBlankAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Exams ex
    LEFT JOIN ExamExercises ee ON ex.Id = ee.ExamId
    LEFT JOIN Exercises e ON ee.ExerciseId = e.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blank Exercise joins
    LEFT JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlankAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE ex.Id = @examId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetExamBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId = @sectionId
END;
GO

CREATE OR ALTER PROCEDURE sp_GetUnassignedExams
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId IS NULL;
END;
GO

CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromExam
    @examId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if exam exists
    IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
    BEGIN
        RAISERROR ('Exam not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists in exam
    IF NOT EXISTS (SELECT 1 FROM ExamExercises WHERE ExamId = @examId AND ExerciseId = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found in this exam.', 16, 1) WITH NOWAIT;
    END

    DELETE FROM ExamExercises
    WHERE ExamId = @examId AND ExerciseId = @exerciseId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateExam
    @examId INT,
    @sectionId INT = NULL
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            RAISERROR ('Exam not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section already has an exam
        IF @sectionId IS NOT NULL AND EXISTS (SELECT 1 FROM Exams WHERE SectionId = @sectionId)
        BEGIN
            RAISERROR ('Section already has an exam', 16, 1) WITH NOWAIT;
        END

        -- Update the exam
        UPDATE Exams
        SET SectionId = @sectionId
        WHERE Id = @examId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_AddExercise
    @type VARCHAR(50),
    @difficultyId INT,
    @question VARCHAR(500),
    -- Multiple Choice parameters
    @correctAnswer VARCHAR(500) = NULL,
    -- Fill in the Blank parameters
    -- Association parameters
    -- Flashcard parameters
    @flashcardAnswer VARCHAR(100) = NULL,
    -- Output parameter
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Validate exercise type
        IF @type NOT IN ('MultipleChoiceExercise', 'FillInTheBlankExercise', 'AssociationExercise', 'FlashcardExercise')
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
        ELSE IF @type = 'FillInTheBlankExercise'
        BEGIN
            -- Insert the fill in the Blank exercise
            INSERT INTO FillInTheBlankExercises (ExerciseId)
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
GO

CREATE OR ALTER PROCEDURE sp_DeleteExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
        BEGIN
            RAISERROR ('Exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the base exercise - cascading will handle the rest
        DELETE FROM Exercises WHERE Id = @exerciseId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllExercises
AS
BEGIN
    SELECT
        e.*,
        d.Name as DifficultyName,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blank Exercise data
        fba.CorrectAnswer as FillInTheBlankAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Exercises e
    LEFT JOIN Difficulties d ON e.DifficultyId = d.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blank Exercise joins
    LEFT JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlankAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
END;
GO

CREATE OR ALTER PROCEDURE sp_GetExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT
        e.*,
        d.Name as DifficultyName,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blank Exercise data
        fba.CorrectAnswer as FillInTheBlankAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Exercises e
    LEFT JOIN Difficulties d ON e.DifficultyId = d.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blank Exercise joins
    LEFT JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlankAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE e.Id = @exerciseId;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddFillInTheBlankAnswer
    @exerciseId INT,
    @correctAnswer VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1
            FROM Exercises e
            JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or not a Fill in the Blank exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the answer
        INSERT INTO FillInTheBlankAnswers (ExerciseId, CorrectAnswer)
        VALUES (@exerciseId, @correctAnswer);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_AddMultipleChoiceOption
    @exerciseId INT,
    @optionText VARCHAR(500)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1
            FROM Exercises e
            JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISERROR('Invalid exercise ID or not a Multiple Choice exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the option
        INSERT INTO MultipleChoiceOptions (ExerciseId, OptionText)
        VALUES (@exerciseId, @optionText);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_AddQuiz
    @sectionId INT = NULL,
    @orderNumber INT = NULL,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists only if sectionId is not NULL
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Insert the new quiz
        INSERT INTO Quizzes (SectionId, OrderNumber)
        VALUES (@sectionId, @orderNumber);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_AddExerciseToQuiz
    @quizId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        RAISERROR ('Quiz not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists
    IF NOT EXISTS (SELECT 1 FROM Exercises WHERE Id = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found.', 16, 1) WITH NOWAIT;
    END

    INSERT INTO QuizExercises (QuizId, ExerciseId)
    VALUES (@quizId, @exerciseId);
END;
GO

CREATE OR ALTER PROCEDURE sp_CountQuizzesBySectionId
    @sectionId INT
AS
BEGIN
    SELECT COUNT(*) AS QuizCount FROM Quizzes
    WHERE SectionId = @sectionId;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteQuiz
    @quizId INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the quiz
        DELETE FROM Quizzes
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    ORDER BY SectionId, OrderNumber;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetQuizById
    @quizId INT
AS
BEGIN
    SELECT q.*
    FROM Quizzes q
    WHERE q.Id = @quizId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetQuizByIdWithExercises
    @quizId INT
AS
BEGIN
    SELECT
        q.*,
        e.Id as ExerciseId,
        e.Type as Type,
        e.Question as Question,
        e.DifficultyId as DifficultyId,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blank Exercise data
        fba.CorrectAnswer as FillInTheBlankAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Quizzes q
    LEFT JOIN QuizExercises qe ON q.Id = qe.QuizId
    LEFT JOIN Exercises e ON qe.ExerciseId = e.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blank Exercise joins
    LEFT JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlankAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE q.Id = @quizId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetQuizzesBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId = @sectionId
    ORDER BY OrderNumber;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetUnassignedQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId IS NULL;
END;
GO

CREATE OR ALTER PROCEDURE sp_LastOrderQuizBySectionId
    @sectionId INT
AS
BEGIN
    SELECT ISNULL(MAX(OrderNumber), 0) AS LastOrderNumber FROM Quizzes
    WHERE SectionId = @sectionId;
END;
GO

CREATE OR ALTER PROCEDURE sp_RemoveExerciseFromQuiz
    @quizId INT,
    @exerciseId INT
AS
BEGIN
    -- Check if quiz exists
    IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
    BEGIN
        RAISERROR ('Quiz not found.', 16, 1) WITH NOWAIT;
    END

    -- Check if exercise exists in quiz
    IF NOT EXISTS (SELECT 1 FROM QuizExercises WHERE QuizId = @quizId AND ExerciseId = @exerciseId)
    BEGIN
        RAISERROR ('Exercise not found in this quiz.', 16, 1) WITH NOWAIT;
    END

    DELETE FROM QuizExercises
    WHERE QuizId = @quizId AND ExerciseId = @exerciseId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateQuiz
    @quizId INT,
    @sectionId INT = NULL,
    @orderNumber INT = NULL
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END
        -- Update the quiz
        UPDATE Quizzes
        SET
            SectionId = @sectionId,
            OrderNumber = @orderNumber
        WHERE Id = @quizId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_AddRoadmap
    @name VARCHAR(100),
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if name already exists
        IF EXISTS (SELECT 1 FROM Roadmaps WHERE Name = @name)
        BEGIN
            RAISERROR ('Roadmap with this name already exists', 16, 1) WITH NOWAIT;
        END

        -- Insert the new roadmap
        INSERT INTO Roadmaps (Name)
        VALUES (@name);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteRoadmap
    @roadmapId INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            RAISERROR ('Roadmap not found', 16, 1) WITH NOWAIT;
        END

        DELETE FROM Roadmaps
        WHERE Id = @roadmapId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetRoadmaps
AS
BEGIN
    SELECT * FROM Roadmaps r
END;
GO

CREATE OR ALTER PROCEDURE sp_GetRoadmapById
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Roadmaps r
    WHERE r.Id = @roadmapId;
END;
GO

CREATE OR ALTER PROCEDURE sp_AddSection
    @subjectId INT,
    @title VARCHAR(255),
    @description VARCHAR(500),
    @roadmapId INT,
    @orderNumber INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if roadmap exists
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            RAISERROR ('Roadmap not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the roadmap
        IF EXISTS (
            SELECT 1
            FROM Sections
            WHERE RoadmapId = @roadmapId
            AND OrderNumber = @orderNumber
        )
        BEGIN
            RAISERROR ('Order number already exists in this roadmap', 16 ,1) WITH NOWAIT;
        END

        -- Insert the new section
        INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber)
        VALUES (@subjectId, @title, @description, @roadmapId, @orderNumber);

        -- Get the new ID
        SET @newId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_CountSectionsByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT COUNT(*) AS SectionCount FROM Sections
    WHERE RoadmapId = @roadmapId;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteSection
    @sectionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Delete all quizzes in this section
        DELETE FROM QuizExercises
        WHERE QuizId IN (SELECT Id FROM Quizzes WHERE SectionId = @sectionId);
        DELETE FROM Quizzes WHERE SectionId = @sectionId;

        -- Delete all exams in this section
        DELETE FROM ExamExercises
        WHERE ExamId IN (SELECT Id FROM Exams WHERE SectionId = @sectionId);
        DELETE FROM Exams WHERE SectionId = @sectionId;

        -- Finally delete the section
        DELETE FROM Sections
        WHERE Id = @sectionId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllSections
AS
BEGIN
    SELECT * FROM Sections
    ORDER BY RoadmapId, OrderNumber;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetSectionById
    @sectionId INT
AS
BEGIN
    SELECT * FROM Sections
    WHERE Id = @sectionId;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetSectionsByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Sections
    WHERE RoadmapId = @roadmapId
    ORDER BY OrderNumber;
END;
GO

CREATE OR ALTER PROCEDURE sp_LastOrderSectionByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT ISNULL(MAX(OrderNumber), 0) AS LastOrderNumber FROM Sections
    WHERE RoadmapId = @roadmapId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateSection
    @sectionId INT,
    @subjectId INT,
    @title VARCHAR(255),
    @description VARCHAR(500),
    @roadmapId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if roadmap exists
        IF NOT EXISTS (SELECT 1 FROM Roadmaps WHERE Id = @roadmapId)
        BEGIN
            RAISERROR ('Roadmap not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the roadmap
        IF EXISTS (
            SELECT 1
            FROM Sections
            WHERE RoadmapId = @roadmapId
            AND OrderNumber = @orderNumber
            AND Id != @sectionId
        )
        BEGIN
            RAISERROR ('Order number already exists in this roadmap', 16, 1) WITH NOWAIT;
        END

        -- Update the section
        UPDATE Sections
        SET
            SubjectId = @subjectId,
            Title = @title,
            Description = @description,
            RoadmapId = @roadmapId,
            OrderNumber = @orderNumber
        WHERE Id = @sectionId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_CreateUser
    @Username VARCHAR(100),
    @newId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR ('Username already exists.', 16, 1) WITH NOWAIT;
    END

    -- Insert new user with default 0 values for progress
    INSERT INTO Users (Username)
    VALUES (@Username);

    -- Return the newly created user's ID
    SET @newId = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetUserById
    @userId INT
AS
BEGIN
    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @userId)
        BEGIN
            RAISERROR ('User not found', 16, 1) WITH NOWAIT;
        END

        -- Get user's last completed section and quiz
        SELECT * FROM Users u
        WHERE u.Id = @userId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE sp_GetUserByUsername
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Users
    WHERE Username = @Username;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateUserProgress
    @Id INT,
    @LastCompletedSectionId INT = NULL,
    @LastCompletedQuizId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET
        NumberOfCompletedSections = COALESCE(@LastCompletedSectionId, NumberOfCompletedSections),
        NumberOfCompletedQuizzesInSection = COALESCE(@LastCompletedQuizId, NumberOfCompletedQuizzesInSection)
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_ProgressUserByOne
    @userId INT
AS
BEGIN
    BEGIN TRY
        DECLARE @currentQuizPos INT, @lastQuizPos INT, @currentSectionId INT;
        SET @userId = 1
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @userId)
        BEGIN
            RAISERROR ('User not found', 16, 1) WITH NOWAIT;
        END

        SELECT @currentSectionId = s.Id
        FROM Sections s
        ORDER BY s.OrderNumber
        OFFSET (SELECT NumberOfCompletedSections FROM Users WHERE Id = @userId) ROWS
        FETCH NEXT 1 ROWS ONLY;

        SELECT @lastQuizPos = COUNT(*)
        FROM Quizzes q
        WHERE SectionId = @currentSectionId

        SELECT @currentQuizPos = NumberOfCompletedQuizzesInSection FROM Users WHERE Id = @userId;

        IF @currentQuizPos >= @lastQuizPos
        BEGIN
            -- Move to the section
            UPDATE Users
            SET NumberOfCompletedQuizzesInSection = 0,
                NumberOfCompletedSections = NumberOfCompletedSections + 1
            WHERE Id = @userId;
        END
        ELSE
        BEGIN
            -- Otherwise, just move to the next quiz in the same section
            UPDATE Users
            SET NumberOfCompletedQuizzesInSection = NumberOfCompletedQuizzesInSection + 1
            WHERE Id = @userId;
        END

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END;
GO