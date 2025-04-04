USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationInsert]
(
     @SmsIntegrationId int 
)

AS

BEGIN

	exec [HA].[LetterHistoryItineraryReservationCxtEmailInsert] @SmsIntegrationId

	exec [HA].[LetterHistoryItineraryReservationIEmailInsert] @SmsIntegrationId
	
	exec [HA].[LetterHistoryItineraryReservationREmailInsert] @SmsIntegrationId
	
	exec [HA].[LetterHistoryItineraryReservationVEmailInsert] @SmsIntegrationId

	-- need to get email request lcode != IEmail,REmail,VEmail 

	exec [HA].[LetterHistoryItineraryReservationZEmailInsert] @SmsIntegrationId

	

	-- not sure of the intent here but it is obviously a no op
	-- update HA.LetterHistory set StatusId = 3 where SmsIntegrationId = @SmsIntegrationId and StatusId = 3

END
GO
