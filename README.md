# UserJourney

CREATE DATABASE UserJourney
USE [UserJourney]
CREATE USER [BUILTIN\Users] FOR LOGIN [BUILTIN\Users] WITH DEFAULT_SCHEMA=[dbo]
ALTER ROLE [db_owner] ADD MEMBER [BUILTIN\Users]

CREATE TABLE [dbo].[Users] (
    [UserId]       INT IDENTITY (1, 1) NOT NULL,
    [FirstName]    VARCHAR (50) NOT NULL,
    [LastName]     VARCHAR (50) NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [PhoneNumber]  VARCHAR (100) NULL,
    [Password]     VARCHAR (50) NOT NULL,
    [PasswordResetToken]     VARCHAR (50) NULL,
    [LastTokenCreatedDate] DATETIME NULL,
	[CreatedDate]  DATETIME NULL,
	[ModifiedDate] DATETIME NULL,
	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);
