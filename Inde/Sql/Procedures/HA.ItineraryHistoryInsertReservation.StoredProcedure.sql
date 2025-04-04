USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsertReservation]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryHistoryInsertReservation]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsertReservation]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [HA].[ItineraryHistoryInsertReservation]
(
	@SmsIntegrationId int
)
AS

BEGIN

	DECLARE @Ids TABLE (Id int)

	INSERT INTO [HA].[ItineraryHistoryReservation]
	(
		[ItineraryHistoryId]
		,[ReservationType]
		,[ReservationId]
	)
	output 
		inserted.ItineraryHistoryReservationId 
	into 
		@Ids 

		   
	select
		ih.ItineraryHistoryId,
		'L'	,
		ir.resno
	from
		ha.ItineraryHistory ih 
		join [pb_springermiller].dbo.in_itinc ic on ic.icode = ih.icode
		join [pb_springermiller].dbo.in_res ir on ir.resno = ic.itcid 
	where
		ih.SmsIntegrationId = @SmsIntegrationId
		and ic.itype = 'R'
		and not exists ( select 1 from [HA].[ItineraryHistoryReservation] res where res.ItineraryHistoryId = ih.ItineraryHistoryId and res.ReservationId = ir.resno and res.ReservationType = 'L' )

	union 

	select
		ih.ItineraryHistoryId,
		'A',	
		rs.sknum
	from
		ha.ItineraryHistory ih 
		join [pb_springermiller].dbo.in_itinc ic on ic.icode = ih.icode
		join [pb_springermiller].dbo.rs_sked rs on rs.sknum = ic.itcid 
	where
		ih.SmsIntegrationId = @SmsIntegrationId
		and ic.itype != 'R'
		and not exists ( select 1 from [HA].[ItineraryHistoryReservation] res where res.ItineraryHistoryId = ih.ItineraryHistoryId and res.ReservationId = rs.sknum and res.ReservationType = 'A' )




	SELECT 
		 a.[ItineraryHistoryReservationId]
		,a.[ItineraryHistoryId]
		,a.[ReservationType]
		,a.[ReservationId]
	FROM 
		[HA].[ItineraryHistoryReservation] a
		join @Ids i on i.Id = a.ItineraryHistoryReservationId


END

GO
