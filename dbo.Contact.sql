CREATE TABLE [dbo].[Contact]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[ContactType] INT NOT NULL,
	[ContactValue] VARCHAR(40) NOT NULL
)
