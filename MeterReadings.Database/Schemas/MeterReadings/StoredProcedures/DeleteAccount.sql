CREATE PROCEDURE [MeterReadings].[DeleteAccount]
	@id int
AS
	DELETE FROM [MeterReadings].[Account] WHERE AccountId = @id
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[DeleteAccount] to[MeterReadingsRole]
AS [dbo]

GO