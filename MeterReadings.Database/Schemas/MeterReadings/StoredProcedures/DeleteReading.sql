CREATE PROCEDURE [MeterReadings].[DeleteReading]
	@id uniqueidentifier
AS
	DELETE FROM [MeterReadings].[Reading] WHERE Id = @id
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[DeleteReading] to[MeterReadingsRole]
AS [dbo]

GO