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