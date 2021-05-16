CREATE PROCEDURE [MeterReadings].[GetAccount]
	@AccountId int
AS
	SELECT * FROM [MeterReadings].[Account] WHERE AccountId = @AccountId
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[GetAccount] to[MeterReadingsRole]
AS [dbo]

GO