USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[UnitTypeDescriptionsGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[UnitTypeDescriptionsGetAll]
GO
/****** Object:  StoredProcedure [HA].[UnitTypeDescriptionsGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [HA].[UnitTypeDescriptionsGetAll]

AS

BEGIN


	select 
			trim(isnull(rmtp.tycod,'')) as tycod, 
			trim(isnull(rmtp.tydes,'')) as tydes, 
			trim(isnull(iu.urating,'')) as urating, 
			trim(isnull(rmtp.tylong,'')) as tylong, 
			trim(isnull(iu.uname,'')) as uname,  
			trim(isnull(iu.unum,'')) as unum
		From 
			PB_SpringerMiller.[dbo].in_unit iu 
			left JOIN PB_SpringerMiller.[dbo].IN_RMTPS rmtp on iu.utype = rmtp.tycod 

	union
		
	select 
			trim(isnull(rmtp.tycod,'')) as tycod, 
			trim(isnull(rmtp.tydes,'')) as tydes, 
			'' as urating, 
			trim(isnull(rmtp.tylong,'')) as tylong, 
			''  as uname,  
			''  as unum
		From 
			 PB_SpringerMiller.[dbo].IN_RMTPS rmtp 
		where 
			rmtp.tycod is not null 
			and trim(isnull(rmtp.tycod,'')) != ''
			and not exists ( select 1 from PB_SpringerMiller.[dbo].in_unit iu where iu.utype =  rmtp.tycod )


order by
	1,6,5

END
GO
