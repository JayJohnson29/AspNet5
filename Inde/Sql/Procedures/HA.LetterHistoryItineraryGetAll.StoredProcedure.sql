USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryGetAll]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create  PROCEDURE [HA].[LetterHistoryItineraryGetAll]
(
     @SmsIntegrationId int 
)

AS

BEGIN

	SELECT
		 [lhi].[LetterHistoryItineraryId]
		,[lhi].[letterhistoryid]
		,[lhi].[lnum]
		,[lhi].[lcode]
		,[lhi].[lguestnum]
		,[lhi].[lmod]
		,[lhi].[lacctnum]
		,[lhi].[icode]
		,[lhi].[iguest]
		,[lhi].[icdate]
		,[lhi].[ictime]
		,[lhi].[iexpdate]
		,[lhi].[iexptime]
		,[lhi].[iarrive]
		,[lhi].[idepart]
		,[lhi].[ibkdprop]
		,[lhi].[icharges]
		,[lhi].[ipayments]
		,[lhi].[iconfnum]
		,[lh].StatusId
		,[lh].StatusMessage
	 FROM
		inntopia.[HA].[LetterHistory] lh
		join inntopia.[HA].[LetterHistoryItinerary] lhi on lhi.letterhistoryid = lh.LetterHistoryId
	WHERE
		lh.SmsIntegrationId = @SmsIntegrationId



END
GO
