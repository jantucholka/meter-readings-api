CREATE PROCEDURE [MeterReadings].[GetAllAccounts]
AS
	SELECT * FROM [MeterReadings].[Account] 
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[GetAllAccounts] to [MeterReadingsRole]
AS [dbo]

GO