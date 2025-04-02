CREATE OR ALTER PROCEDURE sp_GetUserByUsername
    @Username VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Users
    WHERE Username = @Username;
END;
