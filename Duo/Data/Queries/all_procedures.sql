CREATE PROCEDURE sp_CreateUser
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR ('Username already exists.', 16, 1) WITH NOWAIT;
        RETURN;
    END

    -- Insert new user with default NULL values for progress
    INSERT INTO Users (Username, LastCompletedSectionId, LastCompletedQuizId)
    VALUES (@Username, NULL, NULL);
    
    -- Return the newly created user's ID
    SELECT SCOPE_IDENTITY() AS Id;
END
CREATE OR ALTER PROCEDURE sp_GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END; 
CREATE PROCEDURE sp_GetUserById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Username,
        LastCompletedSectionId,
        LastCompletedQuizId
    FROM Users
    WHERE Id = @Id;
END
CREATE PROCEDURE sp_GetUserByUsername
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Username,
        LastCompletedSectionId,
        LastCompletedQuizId
    FROM Users
    WHERE Username = @Username;
END
CREATE OR ALTER PROCEDURE sp_GetUserLastCompleted
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
        SELECT 
            u.Id AS UserId,
            u.Username,
            u.LastCompletedSectionId,
            u.LastCompletedQuizId
        FROM Users u
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
CREATE PROCEDURE sp_UpdateUserProgress
    @Id INT,
    @LastCompletedSectionId INT,
    @LastCompletedQuizId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users
    SET 
        LastCompletedSectionId = @LastCompletedSectionId,
        LastCompletedQuizId = @LastCompletedQuizId
    WHERE Id = @Id;
END
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
CREATE OR ALTER PROCEDURE sp_GetRoadmaps
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Roadmaps r
END;
CREATE OR ALTER PROCEDURE sp_GetRoadmapById
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Roadmaps r
    WHERE r.Id = @roadmapId;
END;
CREATE OR ALTER PROCEDURE sp_GetRoadmapByName
    @roadmapName NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Roadmaps r
    WHERE r.Name = @roadmapName;
END;
CREATE OR ALTER PROCEDURE sp_AddSection
    @subjectId INT,
    @title VARCHAR(255),
    @description TEXT,
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

        -- Update any users who had this as their last completed section
        UPDATE Users
        SET LastCompletedSectionId = NULL
        WHERE LastCompletedSectionId = @sectionId;

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
CREATE OR ALTER PROCEDURE sp_GetAllSections
AS
BEGIN
    SELECT * FROM Sections
    ORDER BY RoadmapId, OrderNumber;
END; 
CREATE OR ALTER PROCEDURE sp_UpdateSection
    @sectionId INT,
    @subjectId INT,
    @title VARCHAR(255),
    @description TEXT,
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
CREATE OR ALTER PROCEDURE sp_GetSectionsByRoadmapId
    @roadmapId INT
AS
BEGIN
    SELECT * FROM Sections
    WHERE RoadmapId = @roadmapId
    ORDER BY OrderNumber;
END; 
CREATE OR ALTER PROCEDURE sp_UpdateSection
    @sectionId INT,
    @subjectId INT,
    @title VARCHAR(255),
    @description TEXT,
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
CREATE OR ALTER PROCEDURE sp_GetAllDifficulties
AS
BEGIN
    SELECT * FROM Difficulties;
END; 
CREATE OR ALTER PROCEDURE sp_GetDifficultyById
    @difficultyId INT
AS
BEGIN
    SELECT * FROM Difficulties
    WHERE Id = @difficultyId;
END; 
CREATE OR ALTER PROCEDURE sp_AddQuiz
    @sectionId INT,
    @orderNumber INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber
        )
        BEGIN
            RAISERROR ('Order number already exists in this section', 16, 1) WITH NOWAIT;
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
CREATE OR ALTER PROCEDURE sp_AddExerciseToQuiz
    @quizId INT,
    @exerciseId INT,
    @newId INT OUTPUT
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

    SET @newId = SCOPE_IDENTITY();
END; 
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

        -- Delete quiz exercises first
        DELETE FROM QuizExercises
        WHERE QuizId = @quizId;

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
CREATE OR ALTER PROCEDURE sp_GetAllQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    ORDER BY SectionId, OrderNumber;
END; 
CREATE OR ALTER PROCEDURE sp_GetQuizById
    @quizId INT
AS
BEGIN
    SELECT q.*
    FROM Quizzes q
    WHERE q.Id = @quizId;
END; 
CREATE OR ALTER PROCEDURE sp_GetQuizByIdWithExercises
    @quizId INT
AS
BEGIN
    SELECT 
        q.*,
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
    FROM Quizzes q
    LEFT JOIN QuizExercises qe ON q.Id = qe.QuizId
    LEFT JOIN Exercises e ON qe.ExerciseId = e.Id
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
    WHERE q.Id = @quizId;
END; 
CREATE OR ALTER PROCEDURE sp_GetQuizzesBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId = @sectionId
    ORDER BY OrderNumber;
END; 
CREATE OR ALTER PROCEDURE sp_GetUnassignedQuizzes
AS
BEGIN
    SELECT *
    FROM Quizzes
    WHERE SectionId IS NULL;
END; 
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
CREATE OR ALTER PROCEDURE sp_UpdateQuiz
    @quizId INT,
    @sectionId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber 
            AND Id != @quizId
        )
        BEGIN
            RAISERROR ('Order number already exists in this section', 16, 1) WITH NOWAIT;
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
CREATE OR ALTER PROCEDURE sp_UpdateExam
    @examId INT,
    @sectionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            RAISERROR ('Exam not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
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
CREATE OR ALTER PROCEDURE sp_GetUnassignedExams
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId IS NULL;
END; 
CREATE OR ALTER PROCEDURE sp_GetExamBySectionId
    @sectionId INT
AS
BEGIN
    SELECT *
    FROM Exams
    WHERE SectionId = @sectionId
END; 
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
CREATE OR ALTER PROCEDURE sp_GetExamById
    @examId INT
AS
BEGIN
    SELECT ex.*
    FROM Exams ex
    WHERE ex.Id = @examId;
END; 
CREATE OR ALTER PROCEDURE sp_UpdateQuiz
    @quizId INT,
    @sectionId INT,
    @orderNumber INT
AS
BEGIN
    BEGIN TRY
        -- Check if quiz exists
        IF NOT EXISTS (SELECT 1 FROM Quizzes WHERE Id = @quizId)
        BEGIN
            RAISERROR ('Quiz not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if order number is unique within the section
        IF EXISTS (
            SELECT 1 
            FROM Quizzes 
            WHERE SectionId = @sectionId 
            AND OrderNumber = @orderNumber 
            AND Id != @quizId
        )
        BEGIN
            RAISERROR ('Order number already exists in this section', 16, 1) WITH NOWAIT;
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
CREATE OR ALTER PROCEDURE sp_AddExam
    @sectionId INT,
    @newId INT OUTPUT
AS
BEGIN
    BEGIN TRY
        -- Check if section exists
        IF NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section already has an exam
        IF EXISTS (SELECT 1 FROM Exams WHERE SectionId = @sectionId)
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
CREATE OR ALTER PROCEDURE sp_AddExerciseToExam
    @examId INT,
    @exerciseId INT,
    @newId INT OUTPUT
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

    SET @newId = SCOPE_IDENTITY();
END; 
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

        -- Delete exam exercises first
        DELETE FROM ExamExercises
        WHERE ExamId = @examId;

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
CREATE OR ALTER PROCEDURE sp_GetAllExams
AS
BEGIN
    SELECT *
    FROM Exams
    ORDER BY SectionId;
END; 
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
CREATE OR ALTER PROCEDURE sp_GetAllExercises
AS
BEGIN
    SELECT 
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
        -- Fill in the Blanks Exercise data
        fba.CorrectAnswer as FillInTheBlanksAnswer,
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
    -- Fill in the Blanks Exercise joins
    LEFT JOIN FillInTheBlanksExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlanksAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE e.Id = @exerciseId;
END; 
CREATE OR ALTER PROCEDURE sp_AddMultipleChoiceExercise
    @exerciseId INT,
    @correctAnswer VARCHAR(500)
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'MultipleChoice'
        )
        BEGIN
            RAISERROR('Exercise not found or not of type MultipleChoice', 16, 1) WITH NOWAIT;
        END

        -- Insert the multiple choice exercise
        INSERT INTO MultipleChoiceExercises (ExerciseId, CorrectAnswer)
        VALUES (@exerciseId, @correctAnswer);
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
CREATE OR ALTER PROCEDURE sp_DeleteMultipleChoiceExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM MultipleChoiceExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISERROR('Multiple Choice exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise (this will cascade delete the options due to foreign key)
        DELETE FROM MultipleChoiceExercises
        WHERE ExerciseId = @exerciseId;
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
CREATE OR ALTER PROCEDURE sp_DeleteMultipleChoiceOption
    @optionId INT
AS
BEGIN
    BEGIN TRY
        -- Check if option exists
        IF NOT EXISTS (
            SELECT 1 
            FROM MultipleChoiceOptions 
            WHERE Id = @optionId
        )
        BEGIN
            RAISERROR('Option not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the option
        DELETE FROM MultipleChoiceOptions
        WHERE Id = @optionId;
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
CREATE OR ALTER PROCEDURE sp_GetMultipleChoiceExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT mce.*, mco.OptionText
    FROM MultipleChoiceExercises mce
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    WHERE mce.ExerciseId = @exerciseId;
END; 
CREATE OR ALTER PROCEDURE sp_AddFlashcardExercise
    @exerciseId INT,
    @answer VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'Flashcard'
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or type', 16, 1) WITH NOWAIT;
        END

        -- Insert the flashcard exercise
        INSERT INTO FlashcardExercises (ExerciseId, Answer)
        VALUES (@exerciseId, @answer);
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
CREATE OR ALTER PROCEDURE sp_DeleteFlashcardExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FlashcardExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISERROR('Flashcard exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise
        DELETE FROM FlashcardExercises
        WHERE ExerciseId = @exerciseId;
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
CREATE OR ALTER PROCEDURE sp_GetFlashcardExercise
    @exerciseId INT
AS
BEGIN
    SELECT 
        fe.*
    FROM FlashcardExercises fe
    WHERE fe.ExerciseId = @exerciseId;
END; 
CREATE OR ALTER PROCEDURE sp_AddFillInTheBlanksAnswer
    @exerciseId INT,
    @correctAnswer VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        -- Validate exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises e
            JOIN FillInTheBlanksExercises fbe ON e.Id = fbe.ExerciseId
            WHERE e.Id = @exerciseId
        )
        BEGIN
            RAISEERROR('Invalid exercise ID or not a Fill in the Blanks exercise', 16, 1) WITH NOWAIT;
        END

        -- Insert the answer
        INSERT INTO FillInTheBlanksAnswers (ExerciseId, CorrectAnswer)
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
CREATE OR ALTER PROCEDURE sp_AddFillInTheBlanksExercise
    @exerciseId INT,
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'FillInTheBlanks'
        )
        BEGIN
            RAISEERROR('Invalid exercise ID or type', 16, 1) WITH NOWAIT;
        END

        -- Insert the fill in the blanks exercise
        INSERT INTO FillInTheBlanksExercises (ExerciseId)
        VALUES (@exerciseId);
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
CREATE OR ALTER PROCEDURE sp_DeleteFillInTheBlanksExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FillInTheBlanksExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISEERROR('Fill in the Blanks exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the exercise (this will cascade delete the answers due to foreign key)
        DELETE FROM FillInTheBlanksExercises
        WHERE ExerciseId = @exerciseId;
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
CREATE OR ALTER PROCEDURE sp_DeleteFillInTheBlanksAnswer
    @answerId INT
AS
BEGIN
    BEGIN TRY
        -- Check if answer exists
        IF NOT EXISTS (
            SELECT 1 
            FROM FillInTheBlanksAnswers 
            WHERE Id = @answerId
        )
        BEGIN
            RAISEERROR('Fill in the Blanks answer not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the answer
        DELETE FROM FillInTheBlanksAnswers
        WHERE Id = @answerId;
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
CREATE OR ALTER PROCEDURE sp_GetFillInTheBlanksExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT fite.*, fita.CorrectAnswer
    FROM FillInTheBlanksExercises fite
    LEFT JOIN FillInTheBlanksAnswers fita ON fite.ExerciseId = fita.ExerciseId
    WHERE fite.ExerciseId = @exerciseId
END; 
CREATE OR ALTER PROCEDURE sp_AddAssociationExercise
    @exerciseId INT,
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists and is of correct type
        IF NOT EXISTS (
            SELECT 1 
            FROM Exercises 
            WHERE Id = @exerciseId 
            AND Type = 'AssociationExercise'
        )
        BEGIN
            RAISERROR ('Invalid exercise ID or type.', 16, 1) WITH NOWAIT;
        END

        -- Insert the association exercise
        INSERT INTO AssociationExercises (ExerciseId)
        VALUES (@exerciseId);
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 
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
CREATE OR ALTER PROCEDURE sp_DeleteAssociationExercise
    @exerciseId INT
AS
BEGIN
    BEGIN TRY
        -- Check if exercise exists
        IF NOT EXISTS (
            SELECT 1 
            FROM AssociationExercises 
            WHERE ExerciseId = @exerciseId
        )
        BEGIN
            RAISERROR ('Association exercise not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the association exercise (this will cascade delete the pairs due to foreign key)
        DELETE FROM AssociationExercises
        WHERE ExerciseId = @exerciseId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 
CREATE OR ALTER PROCEDURE sp_DeleteAssociationPair
    @pairId INT
AS
BEGIN
    BEGIN TRY
        -- Check if pair exists
        IF NOT EXISTS (
            SELECT 1 
            FROM AssociationPairs 
            WHERE Id = @pairId
        )
        BEGIN
            RAISERROR ('Association pair not found', 16, 1) WITH NOWAIT;
        END

        -- Delete the pair
        DELETE FROM AssociationPairs
        WHERE Id = @pairId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 
CREATE OR ALTER PROCEDURE sp_GetAssociationExerciseById
    @exerciseId INT
AS
BEGIN
    SELECT 
        ae.ExerciseId,
        ap.FirstAnswer,
        ap.SecondAnswer
    FROM AssociationExercises ae
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    WHERE ae.ExerciseId = @exerciseId;
END; 