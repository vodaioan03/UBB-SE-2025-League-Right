CREATE OR ALTER PROCEDURE sp_GetUserByUsername
    @username VARCHAR(100)
AS
BEGIN
    SELECT * FROM Users
    WHERE Username = @username;
END; 