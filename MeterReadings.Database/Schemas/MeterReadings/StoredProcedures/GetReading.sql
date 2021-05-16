CREATE PROCEDURE [MeterReadings].[GetReading]
	@id uniqueidentifier
AS
	SELECT * FROM [MeterReadings].[Reading] WHERE Id = @id
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[GetReading] to[MeterReadingsRole]
AS [dbo]

GO