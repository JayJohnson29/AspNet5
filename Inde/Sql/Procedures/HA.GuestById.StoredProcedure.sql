USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[GuestById]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[GuestById]
GO
/****** Object:  StoredProcedure [HA].[GuestById]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--  [dbo].[GetCustomerById] '10M4QR'
create PROCEDURE [HA].[GuestById] (
	@guestnum char(6)
)
AS

BEGIN

select top 1
	trim(isnull(g.guestnum,'')) as guestnum,
	trim(isnull(g.first, '')) as first,
	trim(isnull(g.last, '')) as last,
	trim(isnull(g.title, '')) as title,
	trim(isnull(g.address1, '')) as address1,
	trim(isnull(g.address2,'')) as address2,
	trim(isnull(g.address3, '')) as address3,
	trim(isnull(g.city, '')) as city,
	trim(isnull(g.state, '')) as state,
	trim(isnull(g.zip, '')) as zip,
	trim(isnull(g.country, '')) as country, 
	CAST('1900-01-01' as datetime) as birthday, 
	trim(isnull(g.gender,'')) as gender ,
	trim(isnull(g.phone,'')) as phone,
	trim(isnull(g.nomail,'')) as nomail,
	trim(isnull(g.noemail, '')) as noemail,
	trim(isnull(g.nophone, '')) as nophone,
	trim(isnull(g.ownr, '')) as ownr,
	trim(isnull(g.vip, '')) as vip,
	trim(isnull(g.[source], '')) as [source],
	trim(isnull(g.remarks, '')) as remarks,-- 19
	trim(isnull( e.email,'')) as email,
	isnull( e.[primary],0 ) as [primary],
	trim(isnull( p.pphonenum,'')) as [pphonenum],
	isnull( p.pcode,'Z') as pcode,
	isnull( p.pphtyp, 'P' ) as phonetype

from
	PB_SpringerMiller.dbo.in_guest g
	left join PB_SpringerMiller.dbo.in_email e on e.guestnum = g.guestnum   and ISNULL(e.email, '') != '' and e.rectype = 'G'
	left join PB_SpringerMiller.[dbo].[in_phonm] p on p.[pacctnum] = g.guestnum  and isnull(p.pphonenum,'') != ''  and p.pphtyp != 'F' 
where
	g.guestnum = @guestnum
order by
	e.[primary] desc,
	(
		case  
			when p.pacct = 'G' then 0
			when p.pacct = 'N' then 1
			else 2
		end
	),
	p.pcode,
	(
     CASE
       WHEN p.pphtyp='M' THEN 0
       WHEN p.pphtyp='P' THEN 1
       ELSE 2
     END
) 

	
END
GO
