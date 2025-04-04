USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationGetByLtrHstItineraryId]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationGetByLtrHstItineraryId]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationGetByLtrHstItineraryId]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationGetByLtrHstItineraryId]
(
     @LetterHistoryItineraryId int
)

AS

BEGIN


	SELECT
		[lhir].LetterHistoryItineraryReservationId
		,[Lhir].[ReservationType]
		,lhir.LetterHistoryItineraryId
		,[Lhir].[ReservationId]
	from
		ha.LetterHistoryItineraryReservation lhir  
	WHERE
		lhir.LetterHistoryItineraryId = @LetterHistoryItineraryId
	order by
		lhir.ReservationType desc, lhir.ReservationId 



END
GO
