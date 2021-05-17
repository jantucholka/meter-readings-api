CREATE TABLE [MeterReadings].[Reading]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(), 
    [AccountId] INT NOT NULL, 
    [MeterReadingDateTime] DATETIME NOT NULL, 
    [MeterReadValue] NVARCHAR(50) NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES [MeterReadings].[Account](AccountId)
)
