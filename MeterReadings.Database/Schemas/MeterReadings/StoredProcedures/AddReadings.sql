CREATE TYPE Reading as TABLE (
	[AccountId] int NOT NULL, 
    [MeterReadingDateTime] DATETIME NOT NULL, 
    [MeterReadValue] int NOT NULL);
GO

CREATE PROCEDURE [dbo].[AddReadings]
	@newReadings Reading READONLY
AS
	MERGE [dbo].[Reading] as TARGET
	USING @newReadings as SOURCE
	ON 
		TARGET.AccountId = SOURCE.AccountId AND 
		TARGET.MeterReadingDateTime = SOURCE.MeterReadingDateTime AND 
		TARGET.MeterReadValue = SOURCE.MeterReadValue 
	WHEN NOT MATCHED AND EXISTS (SELECT * FROM [dbo].[Account] WHERE SOURCE.AccountId = AccountId)
		THEN
			INSERT ([AccountId],[MeterReadingDateTime],[MeterReadValue]) 
			VALUES (SOURCE.AccountId, SOURCE.MeterReadingDateTime, SOURCE.MeterReadValue)
	OUTPUT INSERTED.*;
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[dbo].[AddReadings] to[MeterReadingsRole]
AS [dbo]

GO