USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryReservationIds]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryHistoryReservationIds]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryReservationIds]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [HA].[ItineraryHistoryReservationIds]
(
	@SmsIntegrationId int
)
AS

BEGIN

	select 
		ih.icode,
		ihr.ReservationId,
		ihr.ReservationType
	from
		ha.ItineraryHistory ih
		join ha.ItineraryHistoryReservation ihr on ihr.ItineraryHistoryId = ih.ItineraryHistoryId
	where
		ih.SmsIntegrationId = @SmsIntegrationId

END

GO
