USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationREmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationREmailInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationREmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationREmailInsert]
(
   @SMSIntegrationid int 
)
AS

BEGIN


with cte 
( LetterHistoryItineraryid,  lguestnum, icode )
as
(
	select 
		LetterHistoryItineraryid, lhi.lguestnum, lhi.icode 
	from
		inntopia.ha.LetterHistory lh
		join inntopia.ha.LetterHistoryItinerary  lhi on lhi.letterhistoryid = lh.LetterHistoryId
	where
		lh.SmsIntegrationId = @SMSIntegrationId
		and lhi.lcode in ( 'REmail', 'TAREMAIL' )
)



INSERT INTO [HA].[LetterHistoryItineraryReservation]
           ([ReservationType]
           ,LetterHistoryItineraryid
           ,[ReservationId])

-- note: union is distict 
-- non cancelled lodging reservation components 

select 
	'L',
	cte.LetterHistoryItineraryid, 
	ic.itcid
from 
	cte
	join [pb_springermiller].[dbo].in_itinc ic on ic.icode = cte.icode
	join [pb_springermiller].[dbo].in_res ir on ir.resno = ic.itcid  
where 
	 ic.itype = 'R' 
	and ir.level != 'CAN' 
	and ir.guestnum = cte.lguestnum

union 

-- scheduled activity reservations associated with 
-- non cancelled lodging reservation itinerary components 
-- for the itinerary guest
	
select 
	'A',
	LetterHistoryItineraryid, 
	rs.sknum   -- , rs.skdate, rs.sktime 
from 
	cte
	join [pb_springermiller].[dbo].in_itinc ic on ic.icode = cte.icode
	join [pb_springermiller].[dbo].in_res ir on ir.resno = ic.itcid 
	join [pb_springermiller].[dbo].rs_sked rs on rs.skrnum = ir.resno 
where 
	ic.itype = 'R' 
	and ir.level != 'CAN' 
	and ir.guestnum =  cte.lguestnum 
	and rs.sklev in ( 'NEW' ,'CNF','INH','CLA') 

union


-- activity reservation itinerary components for the guest

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

order by
	2,3

	-- update ha.letterhistory status
END
GO
