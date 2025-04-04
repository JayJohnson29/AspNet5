USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationIEmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryReservationIEmailInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryReservationIEmailInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [HA].[LetterHistoryItineraryReservationIEmailInsert]
(
   @SMSIntegrationId int 
)
AS

BEGIN


with cte 
( LetterHistoryItineraryid,  icode )
as
(
	select 
		LetterHistoryItineraryid, lhi.icode 
	from
		inntopia.ha.LetterHistory lh
		join inntopia.ha.LetterHistoryItinerary  lhi on lhi.letterhistoryid = lh.LetterHistoryId
	where
		lh.SmsIntegrationId = @SMSIntegrationId
		and lhi.lcode in ( 'IEmail','CLMAIL' ) 
)



INSERT INTO [HA].[LetterHistoryItineraryReservation]
           ([ReservationType]
           ,[LetterHistoryItineraryId]
           ,[ReservationId])


-- non cancelled lodging reservation components 
select distinct
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

union

-- activity reservation components 


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
	and rs.sklev in ( 'NEW' ,'CNF','INH','CLA') 

--  update ha.LetterHistory status

END
GO
