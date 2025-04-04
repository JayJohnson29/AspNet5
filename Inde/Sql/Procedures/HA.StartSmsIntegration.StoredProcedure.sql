USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[StartSmsIntegration]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[StartSmsIntegration]
GO
/****** Object:  StoredProcedure [HA].[StartSmsIntegration]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [HA].[StartSmsIntegration] (
	@SmsIntegrationStartTime datetime,
	@StageUpdateBeginDate datetime,
	@StageUpdateEndDate datetime,
	@Comment varchar(max),
	@StatusId int
)
AS

BEGIN

	Declare 
		@SmsIntegrationId int


	INSERT INTO [HA].[smsintegration]
           ([SmsIntegrationStartTime]
           ,[StatusId]
           ,[Comment]
           ,[StageUpdateBeginDate]
           ,[StageUpdateEndDate])
     VALUES
           ( @SmsIntegrationStartTime
           ,@StatusId
           ,@Comment
		   ,@StageUpdateBeginDate
		   ,@StageUpdateEndDate )

SELECT @SmsIntegrationId = SCOPE_IDENTITY()

INSERT INTO [ha].[SmsIntegrationTable]
           ([SmsIntegrationId]
           ,[SmsTableId] 
		   ,ProcessComment)

	select
		@SmsIntegrationId,
		SmsTableId,
		''
	from
		SMSTable 
	where
		IsActive = 1
		and IsHistorical = 0


	SELECT 
		[SmsIntegrationId]
		,[SmsIntegrationStartTime]
		,[SmsIntegrationEndTime]
		,[StatusId]
		,[Comment]
		,[StageUpdateBeginDate]
		,[StageUpdateEndDate]
  FROM 
		[HA].[smsintegration]
  Where
		SmsIntegrationId = @SmsIntegrationId



END --proc
GO
