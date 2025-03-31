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