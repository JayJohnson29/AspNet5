USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryArrivalInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryArrivalInsert]
GO
/****** Object:  StoredProcedure [HA].[ItineraryArrivalInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [HA].[ItineraryArrivalInsert]
(
	@SmsIntegrationId int,
	@BeginDate DateTime,
	@EndDate DateTime
)
as
begin

	DECLARE @Ids TABLE (Id int)

	INSERT INTO [HA].[ItineraryArrival]
	(
		[SmsIntegrationId]
		,[BeginDate]
		,[EndDate]
		,StatusId
		,StatusMessage
		,[CreateDate]
		,[UpdateDate]  
	)
	output 
		inserted.ItineraryArrivalId 
	into 
		@Ids 
	VALUES
	(
		@SmsIntegrationId 
		,@BeginDate
		,@EndDate
		,1
		,''
		,getdate()
		,getdate()
	)

	SELECT  
		a.[ItineraryArrivalId]
		,a.[SmsIntegrationId]
		,a.[BeginDate]
		,a.[EndDate]
		,a.[StatusId]
		,a.[StatusMessage]
		,a.[CreateDate]
		,a.[UpdateDate]
	FROM 
		[HA].[ItineraryArrival] a
		join @Ids i on i.Id = a.ItineraryArrivalId

end




GO
