USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryItineraryInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryItineraryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [HA].[LetterHistoryItineraryInsert]
(
     @SmsIntegrationId int 
)

AS

BEGIN

-- Set the itinerary values based on lguestnum where in_wmail.lacctnum = '' 

with cte 
( 
	letterhistoryid
	,[lnum]
	,[lcode]
	,[lguestnum]
	,lmod
	,lacctnum
	,[icode]
	,[iguest]
	,[icdate]
	,[ictime]
	,[iexpdate]
	,[iexptime]
	,[iarrive]
	,[idepart]
	,[ibkdprop]
	,[icharges]
	,[ipayments]
	,[iconfnum]
	,rn
)
as
(
	SELECT 
		 lh.letterhistoryid
		,lh.[lnum]
	    ,w.[lcode]
		,w.lguestnum
		,w.lmod
		,w.lacctnum
		,ih.[icode]
		,ih.iguest
		,ih.[icdate]
		,ih.[ictime]
		,ih.[iexpdate]
		,ih.[iexptime]
		,ih.[iarrive]
		,ih.[idepart]
		,ih.[ibkdprop]
		,ih.[icharges]
		,ih.[ipayments]
		,ih.[iconfnum]
		,ROW_NUMBER() over ( partition by ih.iguest order by ih.icdate desc, ih.ictime desc ) as rn
	FROM 
		inntopia.ha.LetterHistory lh
		join [pb_springermiller].[dbo].[in_wmail] w on lh.lnum = w.lnum
		join [pb_springermiller].[dbo].[in_itinh] ih on ih.iguest = w.lguestnum
	where	
	    lh.SMSIntegrationId = @SmsIntegrationId
		and w.lmod = 'GST'
		-- and lh.StatusId = 1
		
)

INSERT INTO [HA].[LetterHistoryItinerary]
           ([letterhistoryid]
           ,[lnum]
           ,[lcode]
           ,[lguestnum]
           ,[lmod]
           ,[lacctnum]
           ,[icode]
           ,[iguest]
           ,[icdate]
           ,[ictime]
           ,[iexpdate]
           ,[iexptime]
           ,[iarrive]
           ,[idepart]
           ,[ibkdprop]
           ,[icharges]
           ,[ipayments]
           ,[iconfnum])

	SELECT
	    letterhistoryid
		,[lnum]
	    ,[lcode]
		,lguestnum
		,lmod
		,lacctnum
		,isnull( cte.[icode], '' ) as [icode]
		,cte.iguest
		,cte.[icdate]
		,cte.[ictime]
		,cte.[iexpdate]
		,cte.[iexptime]
		,cte.[iarrive]
		,cte.[idepart]
		,cte.[ibkdprop]
		,cte.[icharges]
		,cte.[ipayments]
		,cte.[iconfnum]
	FROM 
		cte
	where
		cte.rn = 1
		and not exists( select 1 from inntopia.ha.LetterHistoryItinerary i where i.letterhistoryid = cte.letterhistoryid )


--- set itinerary based on in_wmail.lacctnum 
;

with cte 
( 
	letterhistoryid
	,[lnum]
	,[lcode]
	,[lguestnum]
	,lmod
	,lacctnum
	,[icode]
	,[iguest]
	,[icdate]
	,[ictime]
	,[iexpdate]
	,[iexptime]
	,[iarrive]
	,[idepart]
	,[ibkdprop]
	,[icharges]
	,[ipayments]
	,[iconfnum]
	,rn
)
as
(
	SELECT 
		 lh.letterhistoryid
		,lh.[lnum]
	    ,w.[lcode]
		,w.lguestnum
		,w.lmod
		,w.lacctnum
		,ih.[icode]
		,ih.iguest
		,ih.[icdate]
		,ih.[ictime]
		,ih.[iexpdate]
		,ih.[iexptime]
		,ih.[iarrive]
		,ih.[idepart]
		,ih.[ibkdprop]
		,ih.[icharges]
		,ih.[ipayments]
		,ih.[iconfnum]
		,ROW_NUMBER() over ( partition by ih.icode order by ih.icdate desc, ih.ictime desc ) as rn
	FROM 
		inntopia.ha.LetterHistory lh
		join [pb_springermiller].[dbo].[in_wmail] w on lh.lnum = w.lnum
		join [pb_springermiller].[dbo].[in_itinc] c on c.itcid = w.lacctnum and c.itype = 'R'
		join [pb_springermiller].[dbo].[in_itinh] ih on ih.icode = c.icode
	where	 
	    lh.SMSIntegrationId = @SmsIntegrationId
		and w.lmod = 'RES'
		-- and lh.StatusId = 1
		
)
INSERT INTO [HA].[LetterHistoryItinerary]
           ([letterhistoryid]
           ,[lnum]
           ,[lcode]
           ,[lguestnum]
           ,[lmod]
           ,[lacctnum]
           ,[icode]
           ,[iguest]
           ,[icdate]
           ,[ictime]
           ,[iexpdate]
           ,[iexptime]
           ,[iarrive]
           ,[idepart]
           ,[ibkdprop]
           ,[icharges]
           ,[ipayments]
           ,[iconfnum])

	SELECT
	    letterhistoryid
		,[lnum]
	    ,[lcode]
		,lguestnum
		,lmod
		,lacctnum
		,isnull( 
		cte.[icode], '' ) as [icode]
		,cte.iguest
		,cte.[icdate]
		,cte.[ictime]
		,cte.[iexpdate]
		,cte.[iexptime]
		,cte.[iarrive]
		,cte.[idepart]
		,cte.[ibkdprop]
		,cte.[icharges]
		,cte.[ipayments]
		,cte.[iconfnum]
	FROM 
		cte
	where
		cte.rn = 1
		and not exists( select 1 from inntopia.ha.LetterHistoryItinerary i where i.letterhistoryid = cte.letterhistoryid )

--	update HA.LetterHistory set StatusId = 2 where SmsIntegrationId = @SmsIntegrationId

END
GO
