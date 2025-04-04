USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryGuestNumbers]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryHistoryGuestNumbers]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryGuestNumbers]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [HA].[ItineraryHistoryGuestNumbers]
(
	@SmsIntegrationId int
)
AS

BEGIN

	select distinct
		ihg.guestnum
	from	
		ha.ItineraryHistory ih
		join ha.ItineraryHistoryGuest ihg on ihg.ItineraryHistoryId = ih.ItineraryHistoryId
	where
		ih.SmsIntegrationId = @SmsIntegrationId

END
GO
