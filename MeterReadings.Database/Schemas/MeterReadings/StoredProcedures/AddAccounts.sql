CREATE TYPE Account as TABLE (
	[AccountId] int NOT NULL, 
    [FirstName] nvarchar(255) NOT NULL, 
    [LastName] nvarchar(255) NOT NULL);
GO

CREATE PROCEDURE [MeterReadings].[AddAccounts]
	@newAccounts Account READONLY
AS
	MERGE [MeterReadings].[Account] as TARGET
	USING @newAccounts as SOURCE
	ON 
		TARGET.AccountId = SOURCE.AccountId AND 
		TARGET.FirstName = SOURCE.FirstName AND 
		TARGET.LastName = SOURCE.LastName 
	WHEN NOT MATCHED
		THEN
			INSERT ([AccountId],[FirstName],[LastName]) 
			VALUES (SOURCE.AccountId, SOURCE.FirstName, SOURCE.LastName)
	OUTPUT INSERTED.*;
RETURN 0

GO

GRANT EXECUTE
ON OBJECT::[MeterReadings].[AddAccounts] to[MeterReadingsRole]
AS [dbo]

GO