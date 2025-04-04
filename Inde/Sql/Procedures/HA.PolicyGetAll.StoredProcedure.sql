USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[PolicyGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[PolicyGetAll]
GO
/****** Object:  StoredProcedure [HA].[PolicyGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [HA].[PolicyGetAll] 

AS
BEGIN

	select  
		ip.plcod, 
		trim(isnull(ip.[pltxt1],'')) as pltxt1, 
		trim(isnull(ip.[pltxt2],'')) as pltxt2, 
		trim(isnull(ip.[pltxt3],'')) as pltxt3, 
		trim(isnull(ip.[pltxt4],'')) as pltxt4
	from 						   
		pb_springermiller.dbo.in_polic ip 
	where
		trim(isnull(ip.plcod,'')) != ''

END
GO
