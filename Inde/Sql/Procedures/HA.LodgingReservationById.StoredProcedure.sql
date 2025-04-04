USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LodgingReservationById]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LodgingReservationById]
GO
/****** Object:  StoredProcedure [HA].[LodgingReservationById]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE  PROCEDURE [HA].[LodgingReservationById]
 (
	@ReservationId varchar(6)
)
AS

BEGIN


	-- Select the reservation data 
	select 
		ir.guestNum,       -- ordinal = 0
		ir.level, 
		ir.resno, 
		ir.arrival, 
		ir.depart, 
		ir.unit,           -- 5
		ir.anum, 
		ir.cnum, 
		ir.units, 
		ir.agent, 
		ir.nights,         -- 10 
		ir.booking, 
		ir.disc, 
		ir.grat, 
		ir.subtotal, 
		ir.extras,         -- 15 
		ir.taxes, 
		ir.total,          -- 17
		ir.charges,
		ir.payments,       -- 19 
		ir.balance,        -- 20 
		ir.lastuser, 
		ir.lastedit, 
		ir.rsource,        -- 23
		ir.package, 
		ir.mrkt,           -- 25 
		ir.submarket,
		ir.version, 
		ir.[group],
		ir.op, 
		ir.onum,           -- 30 
		ir.RSECY, 
		getdate() as CurrentUpdateDate,        -- not sure what to use
		isnull(confs.conf_num,'') as ConfNum, 
		isnull(confs.csystem,'') as ConfSystem, 
		isnull(ir.Special,'') AS Special, -- 35
		ir.depamt, 
		ir.depamt2, 
		ir.unitrtbase, 
		ir.discount as discountPercentage, 
		ir.[name] as ReservationName,           -- 40
		ir.depdue, 
		ir.depdue2, 
		ir.dialed, 
		ir.ptyp, 
		ir.guar,     -- 45
		ir.name2, 
		ir.rnotes, 
		ir.suprate, 
		ir.meal, 
		ir.rcorp,    -- 50 
		ir.featrs, 
		ir.cancel AS CancelDate,
		ir.hnotes, 
		ir.anotes, 
		ir.shrinh,    -- 55
		-- ISNULL(u.uname,'') as unitname, 
		'' as unitname,  -- 8/9/2023 -remove join for one field
		ir.grptotal, 
		ir.crptotal, 
		ir.upgrade,
		ir.ESTARR,   -- 60
		ir.CHOUTTIME,
		ir.share,
        trim(isnull(rates.rdesc,'')) as rdesc, 
        trim(isnull(rates.rtext,'')) as rtext, 
        trim(isnull(rates.ratetypr,'')) as ratetypr, 
        trim(isnull(rates.RFEATRS,'')) as rfeatrs,
        trim(isnull(rates.RTWEBTEXT,'')) as RTWEBTEXT, 
        trim(isnull(rates.rcancpol,'')) as rcancpol , 
        trim(isnull(rates.[rdep1pol],'')) as rdep1pol, 
        trim(isnull(rates.[rdep2pol],'')) as rdep2pol ,
		trim(isnull(grp.gcontact, '')) as grpcontact,
		trim(isnull(grp.gname, '')) as  grpname,
		trim(isnull(grp.gcity, '')) as  grpcity,
		trim(isnull(grp.gstate, '')) as  grpstate,
		trim(isnull(grp.grank, '')) as  grprank,
		trim(isnull(ph.pphonenum, '')) as ContactPhoneNum,
		trim(isnull(ph.pphdescr,'')) as ContactPhoneDescr,
		trim(isnull( ta.aname, '')) as taname,
		trim(isnull( ta.acontact, '')) as tacontact,
		trim(isnull( ta.acity, '')) as tacity,
		trim(isnull( ta.astate, '')) as tastate

	from 
		[pb_springermiller].[dbo].in_res ir
		left join [pb_springermiller].[dbo].in_rates rates on rates.rcode = ir.package
		Outer apply  (select top 1 conf_num, csystem from [pb_springermiller].[dbo].[cr-conf] conf where conf.resno = ir.resno order by addeddt desc ) confs 
		outer apply ( select top 1 gcontact, gname, gCity, gState, grank from [pb_springermiller].[dbo].in_grp g where g.gcode = ir.[group] order by g.glstdate desc ) grp
		outer apply ( select top 1 p.pphonenum, p.pphdescr  from [pb_springermiller].[dbo].in_phonm p  where p.pacctnum = ir.resno and p.pacct = 'N' order by p.pcode ) ph
		outer apply ( select top 1 t.aname, t.acontact, t.acity, t.astate  from [pb_springermiller].[dbo].[ta-agt] t  where t.acode = ir.agent  order by t.aadded desc ) ta
	where 
		ir.resno = @ReservationId

END --proc
GO
