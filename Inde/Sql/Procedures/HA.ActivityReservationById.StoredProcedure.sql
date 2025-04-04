USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ActivityReservationById]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ActivityReservationById]
GO
/****** Object:  StoredProcedure [HA].[ActivityReservationById]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [HA].[ActivityReservationById]
 (
	@ScheduleId varchar(6)
)
AS

BEGIN
 -- [dbo].[usp_GetSchedulesByDateRange] '1900-01-01','1900-01-01'



	select  top 1
		rs.sknum,
		rs.sktype,
		rs.sklev, 
		isnull(rs.skfirst,'') as skfirst,    -- first name
		isnull(rs.sklast,'') as sklast,     -- last name
		isnull(rs.skgnum,'') as skgnum,     -- guest number
		isnull(rs.skppl,0) as skppl,
		isnull(rs.skcnum,0) as skcnum,
		isnull(rs.skdate, '01-01-1900' ) as skdate,
		isnull(rs.sktime,'00:00') as sktime,
		isnull(rs.skroom,'') as skroom,
		isnull(rs.SKDEPAMT,0) as skdepamt,
		isnull(rs.skpromo,'') as skpromo,
		isnull(rs.skprice,0) as skprice,
		isnull(rs.sknote,'') as sknote,
		isnull(rs.skbill, '') as skbill,   -- has service been billed
		isnull(rs.skposrcpt,'') as skposrcpt,
		isnull(rs.skbkop, '') as skbkop,   -- user id of person who booked service
		isnull(rs.skbkdate,'01-01-1900') as skbkdate,  -- booking date
		isnull(rs.skbktime, '00:00' ) as skbktime, -- booking time
		isnull(rs.sklink,'') as sklink,
		isnull(rs.skcart,'') as skcart,
		isnull(rs.skcaddy,'') as skcaddy,
		isnull(rs.skcaddydbl,'') as skcaddydbl,
		isnull(rs.skcaddyfc,'') as skcaddyfc,
		isnull(rs.skclub1,'') as skclub1,
		isnull(rs.skclub2,'') as skclub2,
		isnull(rs.skvip,'') as skvip,
		isnull(rs.skgrate,'') as skgrate,
		isnull(rs.skchlog,'') as skchlog,
		isnull(rs.skhole,'') as skhole,
		isnull(rs.skpkg,'') as skpkg,
		isnull(rs.skmrkt,'') as skmrkt,
		isnull(rs.sksubmrkt,'') as sksubmrkt,
		isnull(rs.skproduct,'') as skproduct,
		isnull(rs.sksdur, 0 ) as sksdur,
		isnull(rs.skrdur, 0 ) as skrdur,
		isnull(room.rrname,'') as rrname,
		isnull(room.rrholes,0) as rrholes,
		isnull(room.rrtxt,'')  as roomTextDescription,
		isnull(im3.brdesc,'') as roomFacilityDescription,
		isnull(im.vdescr,'')  as marketDescription,
		isnull(rs.sksvc,'') as sksvc,
		isnull(svc.rsname, '') as rsname,
		isnull(svc.rsdesc,'' ) as rsdesc,
		isnull(rs.skdepdue,'01-01-1900') as skdepdue,
		isnull(grate.rcourse ,'') as golfRateCourseCode,
        isnull(grate.rdesc,'') as golfRateDescription, 
        isnull(grate.ratetype1,'') as golfRateType,    --- beware PB uses ratetype1 
        isnull(grate.RCANCEL,'') as golfcancpol , 
		isnull(productCodes.cdescr,'') as productDescription
	from 
		[pb_springermiller].[dbo].rs_sked rs 
		left join [pb_springermiller].[dbo].[rs-svc] svc (nolock) on svc.rscode = rs.SKSVC
		left join [pb_springermiller].[dbo].[rs-room] room (nolock) on room.rrcode= rs.skroom
		left join [pb_springermiller].[dbo].in_misc im (nolock) on im.vcode = rs.skmrkt and im.vfile = 'M'
		left join [pb_springermiller].[dbo].[rs-rates] grate on grate.rcode = rs.skgrate                         -- golf rate table
		left join [pb_springermiller].[dbo].[in_misc3] im3 on im3.brcode = room.rrfacil and im3.brfile = 'F'    -- room facility description
		left join [pb_springermiller].[dbo].[in_codes] productCodes on productCodes.ccode = rs.skproduct   -- room facility description
	where 
		rs.sknum = @ScheduleId
	--	and  rs.sktype in ('G' ,'D' ,'I' )
	--	and rs.sklev != 'CAN'
	--order by
	--	rs.skdate asc, rs.sktime asc
END --proc
GO
