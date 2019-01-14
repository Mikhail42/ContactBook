CREATE TABLE [dbo].[Person] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [Birthday]  DATETIME       NULL,
    [Note]      NVARCHAR (400) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

