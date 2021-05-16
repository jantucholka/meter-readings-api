CREATE PROCEDURE [MeterReadings].[GetAllReadings]
AS
	SELECT * FROM [MeterReadings].[Reading] 
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[GetAllReadings] to [MeterReadingsRole]
AS [dbo]

GO