CREATE TABLE [dbo].[Reading]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [AccountId] INT NOT NULL, 
    [MeterReadingDateTime] DATETIME NOT NULL, 
    [MeterReadValue] INT NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
)
