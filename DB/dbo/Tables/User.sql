CREATE TABLE [dbo].[User] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)   NULL,
    [Surname]        VARCHAR (50)   NULL,
    [Email]          VARCHAR (50)   NULL,
    [Phone]          NCHAR (10)     NULL,
    [LastLogon]      DATETIME2 (7)  NULL,
    [CreatedOn]      DATETIME2 (7)  NULL,
    [ActivationCode] INT            NULL,
    [Login]          VARCHAR (50)   NOT NULL,
    [Password]       VARCHAR (50)   NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);



