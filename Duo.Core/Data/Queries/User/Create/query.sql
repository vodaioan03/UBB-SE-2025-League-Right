CREATE PROCEDURE sp_CreateUser
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR ('Username already exists.', 16, 1);
        RETURN;
    END

    -- Insert new user with default NULL values for progress
    INSERT INTO Users (Username, LastCompletedSectionId, LastCompletedQuizId)
    VALUES (@Username, NULL, NULL);
    
    -- Return the newly created user's ID
    SELECT SCOPE_IDENTITY() AS Id;
END
