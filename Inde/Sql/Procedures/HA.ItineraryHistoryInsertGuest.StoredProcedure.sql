USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsertGuest]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryHistoryInsertGuest]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsertGuest]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [HA].[ItineraryHistoryInsertGuest]
(
	@SmsIntegrationId int
)
AS

BEGIN

	--declare @Smsintegrationid int = 58967;
	DECLARE @Ids TABLE (Id int);


	with cte  ( ItineraryHistoryId, guestnum )
	as
	(
	select 
		ih.ItineraryHistoryId,
		--ih.icode,
		in_itinh.iguest
	from
		ha.ItineraryHistory ih
		join [pb_springermiller].dbo.in_itinh on in_itinh.icode = ih.icode
		--join in_guest ig on ig.guestnum = in_itinh.iguest
	where
		ih.SmsIntegrationId = @SmsIntegrationId
		--and not exists ( select 1 from ha.ItineraryHistoryGuest guest where guest.GuestNum = in_itinh.iguest and guest.ItineraryHistoryId = ih.ItineraryHistoryId)

	union
		 
	select 
		ih.ItineraryHistoryId,
		--ih.icode,
		ir.guestnum
	from
		ha.ItineraryHistory ih 
		join [HA].[ItineraryHistoryReservation] ihr on ihr.[ItineraryHistoryId] = ih.ItineraryHistoryId
		join [pb_springermiller].dbo.in_res ir on ir.resno = ihr.reservationId and ihr.reservationtype = 'L'
		--join in_guest ig on ig.guestnum = ir.guestnum
	where
		ih.SmsIntegrationId = @SmsIntegrationId
	--and not exists ( select 1 from ha.ItineraryHistoryGuest guest where guest.GuestNum = ir.guestnum and guest.ItineraryHistoryId = ih.ItineraryHistoryId)

	union 


	select 
		ih.ItineraryHistoryId,
		--ih.icode,
		rs.skgnum
	from
		ha.ItineraryHistory ih 
		join [HA].[ItineraryHistoryReservation] ihr on ihr.[ItineraryHistoryId] = ih.ItineraryHistoryId
		join [pb_springermiller].dbo.rs_sked rs on rs.sknum = ihr.reservationid and ihr.reservationtype	= 'A'
		--join in_guest ig on ig.guestnum = rs.skgnum
	where
		ih.SmsIntegrationId = @SmsIntegrationId
			--	and not exists ( select 1 from ha.ItineraryHistoryGuest guest where guest.GuestNum = rs.skgnum and guest.ItineraryHistoryId = ih.ItineraryHistoryId)
	)


	INSERT INTO [HA].[ItineraryHistoryGuest]
	(
		[ItineraryHistoryId]
		,GuestNum
	)
	output 
		inserted.ItineraryHistoryGuestId 
	into 
		@Ids 

	select 
		cte.ItineraryHistoryId,
		--cte.icode,
		cte.guestnum
		--ROW_NUMBER() over ( partition by cte.icode order by cte.icode ) as rn
	from	
		cte
	where
		not exists ( select 1 from ha.ItineraryHistoryGuest guest where guest.GuestNum = cte.guestnum and guest.ItineraryHistoryId = cte.ItineraryHistoryId)


	SELECT 
		 a.[ItineraryHistoryGuestId]
		,a.[ItineraryHistoryId]
		,a.[GuestNum]
	FROM 
		[HA].[ItineraryHistoryGuest] a
		join @Ids i on i.Id = a.ItineraryHistoryGuestId

END
GO
