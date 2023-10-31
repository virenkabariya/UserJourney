# UserJourney

=> For run this project you need to run below script on your database.

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

=> Also You need to change ConnectionStrings & EMailSettings on appsettings.json

=> UserJourney.Site is developed using .NET Core 2.1. It is basically a view and a controller.
=> UserJourney.Repositories is a class library; in this, we have to connect to the database using the entity framework.
=> UserJourney.Core is a class library; in this, we have added common services, enums, templates, etc.
