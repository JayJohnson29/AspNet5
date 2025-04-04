USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterRequestInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterRequestInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterRequestInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [HA].[LetterRequestInsert]
(
	@SmsIntegrationId int,
	@BeginDate datetime,
	@EndDate datetime
)
AS

BEGIN

-- 1) Populate Letter History
	exec [HA].[LetterHistoryInsert] @SmsIntegrationId

-- 2) Populate Letter History Itinerary
	exec [HA].[LetterHistoryItineraryInsert] @SmsIntegrationId

-- 3) populate Letter History Itinerary Reservation

	exec [HA].[LetterHistoryItineraryReservationInsert] @SmsIntegrationId

-- 4) populate Reservation tables  
--    LodgingReservation and ActivityReservation
--	exec [HA].[LetterHistoryItineraryReservationLodgingInsert] @SmsIntegrationId
--	exec [HA].[LetterHistoryItineraryReservationLodgingSpecialBillingInsert]
--	exec [HA].[LetterHistoryItineraryReservationLodgingNotesInsert] 'N'
--	exec [HA].[LetterHistoryItineraryReservationLodgingUpdatePolicy]
--	exec [HA].[LetterHistoryItineraryReservationLodgingUpdateUnitDetails]

----  ActivityReservation

--	exec [HA].[LetterHistoryItineraryReservationActivityInsert] @SmsIntegrationId

	DECLARE @Ids TABLE (Id int)

	INSERT INTO inntopia.[HA].[LetterRequest]
           ([SmsIntegrationId]
           ,[BeginDate]
           ,[EndDate]
           ,[StatusId]
           ,[StatusMessage]
           ,[CreateDate]
           ,[UpdateDate])
	output 
		inserted.LetterRequestId 
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
		a.[LetterRequestId]
		,a.[SmsIntegrationId]
		,a.[BeginDate]
		,a.[EndDate]
		,a.[StatusId]
		,a.[StatusMessage]
		,a.[CreateDate]
		,a.[UpdateDate]
	FROM 
		inntopia.[HA].[LetterRequest] a
		join @Ids i on i.Id = a.LetterRequestId

END
GO
