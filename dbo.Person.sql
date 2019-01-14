CREATE TABLE [dbo].[Person] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (30)  NULL,
    [LastName]  NVARCHAR (30)  NULL,
    [MidleName] NVARCHAR (50)  NULL,
    [Birthday]  DATETIME       NULL,
    [Note]      NVARCHAR (400) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

