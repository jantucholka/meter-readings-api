CREATE TYPE Reading as TABLE (
	[AccountId] int NOT NULL, 
    [MeterReadingDateTime] DATETIME NOT NULL, 
    [MeterReadValue] int NOT NULL);
GO

CREATE PROCEDURE [MeterReadings].[AddReadings]
	@newReadings Reading READONLY
AS
	MERGE [MeterReadings].[Reading] as TARGET
	USING @newReadings as SOURCE
	ON 
		TARGET.AccountId = SOURCE.AccountId AND 
		TARGET.MeterReadingDateTime = SOURCE.MeterReadingDateTime AND 
		TARGET.MeterReadValue = SOURCE.MeterReadValue 
	WHEN NOT MATCHED AND EXISTS (SELECT * FROM [MeterReadings].[Account] WHERE SOURCE.AccountId = AccountId)
		THEN
			INSERT ([Id],[AccountId],[MeterReadingDateTime],[MeterReadValue]) 
			VALUES (NEWID(), SOURCE.AccountId, SOURCE.MeterReadingDateTime, SOURCE.MeterReadValue)
	OUTPUT INSERTED.*;
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[AddReadings] to[MeterReadingsRole]
AS [dbo]

GO