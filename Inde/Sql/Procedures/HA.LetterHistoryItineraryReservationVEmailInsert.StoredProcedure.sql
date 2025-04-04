USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationVEmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationVEmailInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationVEmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationVEmailInsert]
(
    @SMSIntegrationId int 
)
AS

BEGIN


with cte 
	( LetterHistoryItineraryid,  lguestnum, icode, iarrive )
as
(
	select 
		LetterHistoryItineraryid, lhi.lguestnum, lhi.icode, lhi.iarrive
	from
		inntopia.ha.LetterHistory lh
		join inntopia.ha.LetterHistoryItinerary  lhi on lhi.letterhistoryid = lh.LetterHistoryId
	where
		lh.SmsIntegrationId = @SMSIntegrationId
		and lhi.lcode in ( 'VEmail', 'VEIAGT', 'SPEVMAIL' )
)


INSERT INTO [HA].[LetterHistoryItineraryReservation]
           ([ReservationType]
           ,LetterHistoryItineraryid
           ,[ReservationId])



-- non cancelled actvity reservation itinerary components

select  
	'A',
	LetterHistoryItineraryid, 
	rs.sknum       -- , rs.skdate, rs.sktime 
from 
	cte
	join [pb_springermiller].[dbo].in_itinc ic on ic.icode = cte.icode
	join [pb_springermiller].[dbo].rs_sked rs on rs.sknum = ic.itcid 
where 
	ic.itype != 'R' 
	and rs.skgnum = cte.lguestnum
	and rs.sklev in ( 'NEW' ,'CNF','INH','CLA') 

union

--  activity reservations for the guest

select  
	'A',
	LetterHistoryItineraryid, 
	rs.sknum       -- , rs.skdate, rs.sktime 
from 
	cte
	join [pb_springermiller].[dbo].rs_sked rs on rs.skgnum = cte.lguestnum -- '{request.iguest}' 
where 
	 rs.sklev in ( 'NEW' ,'CNF','INH','CLA') 
	and rs.skdate >= cte.iarrive

order by
	2, 3
END
GO
