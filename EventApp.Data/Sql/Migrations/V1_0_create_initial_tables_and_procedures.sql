IF NOT EXISTS (select * from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'Event')
BEGIN
	CREATE TABLE [dbo].[Event](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NULL,
		[Location] [nvarchar](100) NOT NULL,
		[StartTime] [DateTime] NULL,
		[EndTime] [DateTime] NULL,
		CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT UN_Name UNIQUE([Name]) 
	)

	INSERT INTO [dbo].[Event]
           ([Name]
           ,[Description]
           ,[Location]
           ,[StartTime]
           ,[EndTime])
     VALUES
           ('test event 1'
           ,'test event 1 description'
		   ,'Munich'
           ,'2021-10-21 10:00:00'
           ,'2021-10-21 12:00:00')

	INSERT INTO [dbo].[Event]
           ([Name]
           ,[Description]
           ,[Location]
           ,[StartTime]
           ,[EndTime])
     VALUES
           ('test event 2'
           ,'test event 2 description'
		   ,'Prague'
           ,'2021-10-22 20:00:00'
           ,'2021-10-22 22:00:00')
END


---------------
IF NOT EXISTS (select * from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'Registration')
BEGIN

CREATE TABLE [dbo].[Registration](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[EventId] [INT] NOT NULL,
	[Name] [NVARCHAR](100) NOT NULL,
	[Phone] [NVARCHAR](20) NULL,
	[Email] [NVARCHAR](100) NOT NULL,
	CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([Id] ASC)
)

ALTER TABLE [dbo].[Registration] WITH CHECK ADD CONSTRAINT [FK_Registration_Event] FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([Id])

ALTER TABLE [dbo].[Registration] CHECK CONSTRAINT [FK_Registration_Event]

INSERT INTO [dbo].[Registration]
        ([Name]
        ,[EventId]
        ,[Phone]
        ,[Email])
    VALUES
        ('test registration 1'
        ,(SELECT TOP 1 Id FROM [dbo].[Event] WHERE [Name] = 'test event 2')
		,'+49 888 555 000'
        ,'test1@gmail.com')

INSERT INTO [dbo].[Registration]
        ([Name]
        ,[EventId]
        ,[Phone]
        ,[Email])
    VALUES
        ('test registration 2'
        ,(SELECT TOP 1 Id FROM [dbo].[Event] WHERE [Name] = 'test event 2')
		,'+49 555 777 444'
        ,'test2@gmail.com')

INSERT INTO [dbo].[Registration]
        ([Name]
        ,[EventId]
        ,[Phone]
        ,[Email])
    VALUES
        ('test registration 3'
        ,(SELECT TOP 1 Id FROM [dbo].[Event] WHERE [Name] = 'test event 2')
		,'+49 222 333 222'
        ,'test3@gmail.com')
END


--------------------------------------------------------------------------------
--                                sp_GetEvents                                --
--------------------------------------------------------------------------------
IF EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'sp_GetEvents')
			AND TYPE IN (
				N'P'
				,N'PC'
				)
		)
BEGIN
	DROP PROCEDURE [sp_GetEvents]
END
GO

CREATE PROCEDURE [sp_GetEvents]
AS
BEGIN
	SELECT e.[Id], e.[Name], e.[Description], e.[Location], e.StartTime, e.EndTime
	FROM [Event] e
END
GO

--------------------------------------------------------------------------------
--                                sp_GetRegistrations                         --
--------------------------------------------------------------------------------
IF EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'sp_GetRegistrations')
			AND TYPE IN (
				N'P'
				,N'PC'
				)
		)
BEGIN
	DROP PROCEDURE [sp_GetRegistrations]
END
GO

CREATE PROCEDURE [sp_GetRegistrations]
(
	@eventName NVARCHAR(100)
)
AS
BEGIN
	SELECT r.[Id], r.[Name], r.[Phone], r.[Email]
	FROM [dbo].[Registration] r
	INNER JOIN [dbo].[Event] e 
	ON r.EventId = e.Id
	WHERE e.[Name] = @eventName
END
GO

--------------------------------------------------------------------------------
--                                sp_InsertRegistration                       --
--------------------------------------------------------------------------------
IF EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'sp_InsertRegistration')
			AND TYPE IN (
				N'P'
				,N'PC'
				)
		)
BEGIN
	DROP PROCEDURE [sp_InsertRegistration]
END
GO

CREATE PROCEDURE [sp_InsertRegistration]
(
	@name NVARCHAR(100),
	@phone NVARCHAR(20),
	@email NVARCHAR(100),
	@eventName NVARCHAR(100)
)
AS
BEGIN
	INSERT INTO [dbo].[Registration]
			([Name]
			,[EventId]
			,[Phone]
			,[Email])
		VALUES
			(@name
			,(SELECT TOP 1 Id FROM [dbo].[Event] WHERE [Name] = @eventName)
			,@phone
			,@email)

	SELECT @@IDENTITY
END
GO


 --------------------------------------------------------------------------------
--                                sp_CheckUniqueRegistration                         --
--------------------------------------------------------------------------------
IF EXISTS (
		SELECT 1
		FROM sys.objects
		WHERE object_id = OBJECT_ID(N'sp_CheckUniqueRegistration')
			AND TYPE IN (
				N'P'
				,N'PC'
				)
		)
BEGIN
	DROP PROCEDURE [sp_CheckUniqueRegistration]
END
GO

CREATE PROCEDURE [sp_CheckUniqueRegistration]
(
	@phone NVARCHAR(20),
	@email NVARCHAR(100),
	@eventName NVARCHAR(100)
)
AS
BEGIN
	SELECT COUNT(*) FROM [Registration] r 
	INNER JOIN [Event] e ON r.EventId = e.Id 
	WHERE e.[Name] = @eventName
	AND (Phone = @phone OR Email = @email)
END
GO
