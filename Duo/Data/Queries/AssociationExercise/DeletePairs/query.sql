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
            THROW 50001, 'Association pair not found', 1;
        END

        -- Delete the pair
        DELETE FROM AssociationPairs
        WHERE Id = @pairId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END; 