CREATE TABLE [dbo].[Person]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FirstName] VARCHAR(30),
	[LastName] VARCHAR(30),
	[MidleName] VARCHAR(50),
	[Birthday] DATETIME,
	[Note] VARCHAR(400)
)
