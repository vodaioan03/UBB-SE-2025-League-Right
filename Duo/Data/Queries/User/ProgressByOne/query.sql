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