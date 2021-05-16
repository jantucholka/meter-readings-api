CREATE PROCEDURE [MeterReadings].[GetAccount]
	@id int
AS
	SELECT * FROM [MeterReadings].[Account] WHERE AccountId = @id
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[GetAccount] to[MeterReadingsRole]
AS [dbo]

GO