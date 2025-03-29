CREATE OR ALTER PROCEDURE sp_GetUserById
    @userId INT
AS
BEGIN
    SELECT * FROM Users
    WHERE Id = @userId;
END; 