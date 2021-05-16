CREATE PROCEDURE [MeterReadings].[DeleteAccount]
	@AccountId int
AS
	DELETE FROM [MeterReadings].[Account] WHERE AccountId = @AccountId
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[DeleteAccount] to[MeterReadingsRole]
AS [dbo]

GO