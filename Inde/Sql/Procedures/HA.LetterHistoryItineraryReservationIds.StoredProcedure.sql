USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationIds]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationIds]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationIds]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationIds]
(
     @SmsIntegrationId int,
	 @Lnum varchar(6)
)

AS

BEGIN


	SELECT
		[Lhir].[ReservationType]
		,[Lhir].[ReservationId]
	from
		ha.LetterHistory lh
		join ha.letterhistoryitinerary lhi on lhi.letterhistoryid = lh.LetterHistoryId 
		join ha.LetterHistoryItineraryReservation lhir on lhir.LetterHistoryItineraryId = lhi.LetterHistoryItineraryId
	WHERE
		lh.SmsIntegrationId = @SmsIntegrationId
		and lhi.lnum = @Lnum
	order by
		lhir.ReservationType



END
GO
