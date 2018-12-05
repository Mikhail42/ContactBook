CREATE TABLE [dbo].[PersonContact]
(
	[ContactId] INT NOT NULL PRIMARY KEY,
	[PersonId] INT NOT NULL PRIMARY KEY,
	FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person]([Id]),
	FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact]([Id])
)
